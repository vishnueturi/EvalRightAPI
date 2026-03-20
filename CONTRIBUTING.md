# Contributing to EvalRight API

Thank you for your interest in contributing! Here's how to get started.

## Code of Conduct

Please be respectful and constructive in all interactions.

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/your-username/evalright-api.git`
3. Create a feature branch: `git checkout -b feature/amazing-feature`
4. Make your changes
5. Commit: `git commit -m 'Add amazing feature'`
6. Push: `git push origin feature/amazing-feature`
7. Open a Pull Request

## Development Setup

```bash
# Install dependencies
dotnet restore

# Build the project
dotnet build

# Run the API
dotnet run

# Run tests
dotnet test
```

## Code Style

- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use `async`/`await` for all I/O operations
- Add XML documentation comments for public APIs
- Keep methods focused and testable

## Commit Messages

Use clear, descriptive commit messages:
- ✅ `Fix: Resolve null reference in token response handling`
- ✅ `Feature: Add request body size validation`
- ❌ `fixed bug`
- ❌ `updates`

## Pull Request Process

1. Update README.md if you're adding features
2. Ensure all tests pass: `dotnet test`
3. Keep PRs focused on a single issue or feature
4. Provide clear description of changes
5. Reference related issues: `Closes #123`

## Reporting Issues

- Check existing issues first
- Provide clear reproduction steps
- Include environment details (.NET version, OS)
- Attach logs if relevant

## Security

- Never commit secrets or credentials
- Report security issues privately to maintainers
- Don't commit API keys or sensitive tokens

Thank you for contributing! 🙏
