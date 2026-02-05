var builder = WebApplication.CreateBuilder(args);

// Minimal API для .NET 10
var visitCount = 0;
var deploymentId = Guid.NewGuid().ToString("N")[..8];
var startTime = DateTime.UtcNow;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// 📊 API Endpoints
app.MapGet("/api/visit", () =>
{
    Interlocked.Increment(ref visitCount);
    
    return Results.Ok(new
    {
        visitNumber = visitCount,
        message = "🎉 Welcome to .NET 10 Visitor Counter!",
        serverTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC"),
        deploymentId = deploymentId,
        version = "2.0.0",
        framework = ".NET 10",
        uptime = (DateTime.UtcNow - startTime).ToString(@"dd\.hh\:mm\:ss"),
        health = "🟢 OK"
    });
});

app.MapGet("/api/visit/stats", () =>
{
    var uptime = DateTime.UtcNow - startTime;
    var visitsPerMinute = uptime.TotalMinutes > 0 
        ? Math.Round(visitCount / uptime.TotalMinutes, 2) 
        : 0;
    
    return Results.Ok(new
    {
        totalVisits = visitCount,
        visitsPerMinute = visitsPerMinute,
        deploymentTime = startTime.ToString("yyyy-MM-dd HH:mm:ss"),
        deploymentId = deploymentId,
        uptime = uptime.ToString(@"dd\.hh\:mm\:ss"),
        currentDeployment = true
    });
});

app.MapGet("/api/visit/reset", () =>
{
    visitCount = 0;
    deploymentId = Guid.NewGuid().ToString("N")[..8];
    startTime = DateTime.UtcNow;
    
    return Results.Ok(new
    {
        message = "🚀 New deployment detected! Counter reset.",
        newDeploymentId = deploymentId,
        resetTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
    });
});

// 🎨 HTML страница
app.MapGet("/", async (HttpContext context) =>
{
    var html = @"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>.NET 10 Visitor Counter</title>
        <style>
            * { margin: 0; padding: 0; box-sizing: border-box; }
            body {
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, sans-serif;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                min-height: 100vh;
                display: flex;
                justify-content: center;
                align-items: center;
                padding: 20px;
            }
            .container {
                background: rgba(255, 255, 255, 0.95);
                backdrop-filter: blur(10px);
                border-radius: 20px;
                padding: 40px;
                box-shadow: 0 20px 60px rgba(0,0,0,0.3);
                max-width: 800px;
                width: 100%;
            }
            .header {
                text-align: center;
                margin-bottom: 30px;
            }
            .badge {
                display: inline-block;
                background: #10b981;
                color: white;
                padding: 5px 15px;
                border-radius: 20px;
                font-size: 14px;
                font-weight: 600;
                margin-bottom: 10px;
            }
            h1 {
                font-size: 2.5rem;
                color: #1f2937;
                margin-bottom: 10px;
            }
            .counter {
                font-size: 5rem;
                font-weight: 800;
                text-align: center;
                color: #3b82f6;
                margin: 30px 0;
                text-shadow: 2px 2px 4px rgba(0,0,0,0.1);
            }
            .info {
                background: #f3f4f6;
                border-radius: 10px;
                padding: 20px;
                margin: 20px 0;
            }
            .info p {
                margin: 10px 0;
                font-size: 1rem;
                color: #4b5563;
            }
            .info strong {
                color: #1f2937;
            }
            .buttons {
                display: flex;
                gap: 15px;
                justify-content: center;
                margin: 30px 0;
                flex-wrap: wrap;
            }
            button {
                background: #3b82f6;
                color: white;
                border: none;
                padding: 15px 30px;
                border-radius: 10px;
                font-size: 1rem;
                font-weight: 600;
                cursor: pointer;
                transition: all 0.3s ease;
                display: flex;
                align-items: center;
                gap: 10px;
            }
            button:hover {
                background: #2563eb;
                transform: translateY(-2px);
                box-shadow: 0 10px 20px rgba(59, 130, 246, 0.3);
            }
            button:active {
                transform: translateY(0);
            }
            .stats {
                margin-top: 30px;
                padding-top: 20px;
                border-top: 2px solid #e5e7eb;
            }
            .stats h3 {
                color: #1f2937;
                margin-bottom: 15px;
            }
            .footer {
                text-align: center;
                margin-top: 30px;
                color: #6b7280;
                font-size: 0.9rem;
            }
            .deployment-id {
                background: #fef3c7;
                color: #92400e;
                padding: 10px;
                border-radius: 8px;
                font-family: monospace;
                margin: 10px 0;
            }
            .ci-status {
                display: inline-flex;
                align-items: center;
                gap: 5px;
                padding: 5px 10px;
                border-radius: 5px;
                font-size: 0.9rem;
            }
            .ci-success {
                background: #d1fae5;
                color: #065f46;
            }
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <span class='badge'>.NET 10.0</span>
                <h1>Visitor Counter with CI/CD</h1>
                <p>Watch the counter reset on every deployment!</p>
            </div>
            
            <div class='counter' id='counter'>Loading...</div>
            
            <div class='info'>
                <p><strong>How it works:</strong> Each visit increments the counter. When CI/CD deploys new version, counter resets to 0!</p>
                <p><strong>Deployment ID:</strong> <span class='deployment-id' id='deploymentId'>-</span></p>
                <p><strong>Uptime:</strong> <span id='uptime'>-</span></p>
                <p class='ci-status ci-success'>CI/CD: Active | GitHub Actions</p>
            </div>
            
            <div class='buttons'>
                <button onclick='visit()'>Visit Again</button>
                <button onclick='getStats()'>Show Stats</button>
                <button onclick='simulateDeploy()'>Simulate Deployment</button>
                <button onclick='location.reload()'>Refresh Page</button>
            </div>
            
            <div class='stats' id='stats'></div>
            
            <div class='footer'>
                <p>Built with .NET 10 Minimal APIs | GitHub Actions CI/CD</p>
                <p>Each deployment generates new Deployment ID</p>
            </div>
        </div>
        
        <script>
            async function visit() {
                try {
                    const response = await fetch('/api/visit');
                    const data = await response.json();
                    updateDisplay(data);
                } catch (error) {
                    console.error('Error:', error);
                }
            }
            
            async function getStats() {
                try {
                    const response = await fetch('/api/visit/stats');
                    const data = await response.json();
                    showStats(data);
                } catch (error) {
                    console.error('Error:', error);
                }
            }
            
            async function simulateDeploy() {
                try {
                    const response = await fetch('/api/visit/reset');
                    const data = await response.json();
                    alert('Simulated Deployment!\n' + data.message);
                    visit(); 
                } catch (error) {
                    console.error('Error:', error);
                }
            }
            
            function updateDisplay(data) {
                document.getElementById('counter').textContent = 
                    `${data.visitNumber} visits`;
                document.getElementById('deploymentId').textContent = data.deploymentId;
                document.getElementById('uptime').textContent = data.uptime;
                
                document.getElementById('stats').innerHTML = \`
                    <h3>Current Session</h3>
                    <p><strong>Server Time:</strong> \${data.serverTime}</p>
                    <p><strong>Version:</strong> \${data.version} (\${data.framework})</p>
                    <p><strong>Health:</strong> \${data.health}</p>
                \`;
            }
            
            function showStats(data) {
                document.getElementById('stats').innerHTML = \`
                    <h3>Statistics</h3>
                    <p><strong>Total Visits:</strong> \${data.totalVisits}</p>
                    <p><strong>Visits per Minute:</strong> \${data.visitsPerMinute}</p>
                    <p><strong>Deployment Time:</strong> \${data.deploymentTime}</p>
                    <p><strong>Uptime:</strong> \${data.uptime}</p>
                    <p><strong>Deployment ID:</strong> <span class='deployment-id'>\${data.deploymentId}</span></p>
                \`;
            }
            

            visit();
            

            setInterval(visit, 30000);
        </script>
    </body>
    </html>";
    
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.Run();