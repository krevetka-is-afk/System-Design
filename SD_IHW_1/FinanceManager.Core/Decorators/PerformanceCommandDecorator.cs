using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FinanceManager.Core.Commands;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Core.Decorators
{
    public class PerformanceCommandDecorator<T> : ICommand<T>
    {
        private readonly ICommand<T> _command;
        private readonly ILogger<PerformanceCommandDecorator<T>> _logger;
        private readonly string _commandName;

        public PerformanceCommandDecorator(ICommand<T> command, ILogger<PerformanceCommandDecorator<T>> logger)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandName = command.GetType().Name;
        }

        public async Task<T> ExecuteAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var result = await _command.ExecuteAsync();
                stopwatch.Stop();
                _logger.LogInformation("{CommandName} executed in {ElapsedMilliseconds}ms", _commandName, stopwatch.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{CommandName} failed after {ElapsedMilliseconds}ms", _commandName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    public class PerformanceCommandDecorator : ICommand
    {
        private readonly ICommand _command;
        private readonly ILogger<PerformanceCommandDecorator> _logger;
        private readonly string _commandName;

        public PerformanceCommandDecorator(ICommand command, ILogger<PerformanceCommandDecorator> logger)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandName = command.GetType().Name;
        }

        public async Task ExecuteAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _command.ExecuteAsync();
                stopwatch.Stop();
                _logger.LogInformation("{CommandName} executed in {ElapsedMilliseconds}ms", _commandName, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{CommandName} failed after {ElapsedMilliseconds}ms", _commandName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
} 