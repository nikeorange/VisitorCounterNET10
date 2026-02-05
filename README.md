# 🚀 .NET 10 Visitor Counter with CI/CD

[![.NET 10 CI/CD](https://github.com/YOUR_USERNAME/VisitorCounterNET10/actions/workflows/deploy.yml/badge.svg)](https://github.com/YOUR_USERNAME/VisitorCounterNET10/actions/workflows/deploy.yml)

Проект демонстрирующий **CI/CD в действии** на .NET 10. Каждое развертывание сбрасывает счетчик посещений!

## 🎯 Что можно увидеть

После каждого деплоя через CI/CD:

1. **🔢 Счетчик посещений обнуляется** - начинает с 1
2. **🆔 Генерируется новый Deployment ID**
3. **⏰ Сбрасывается время работы приложения**
4. **✅ Все шаги CI/CD видны в GitHub Actions**

## 🔧 Технологии

- **.NET 10.0** - последняя версия
- **Minimal APIs** - современный подход
- **GitHub Actions** - CI/CD пайплайн
- **Docker** - контейнеризация
- **HTML/JS** - фронтенд интерфейс

## 🚀 Как запустить локально

```bash
# Клонировать репозиторий
git clone https://github.com/YOUR_USERNAME/VisitorCounterNET10.git
cd VisitorCounterNET10

# Запустить приложение
dotnet run --project VisitorCounter

# Открыть в браузере
# http://localhost:8080
# http://localhost:8080/api/visit