using Microsoft.Extensions.Logging;
using Moq;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Mappings;
using TaskManagement.Application.Requests.Task;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities.Task;
using TaskManagement.Domain.Enum;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Result;

namespace TaskManagement.Api.UnitTest.TaskManagement.Application
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly Mock<ITaskValidator> _mockValidator;
        private readonly Mock<ILogger<TaskService>> _mockLogger;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _mockValidator = new Mock<ITaskValidator>();
            _mockLogger = new Mock<ILogger<TaskService>>();
            _taskService = new TaskService(_mockRepository.Object, _mockValidator.Object, _mockLogger.Object);
        }

        #region CreateTask Tests

        [Fact]
        public async Task CreateTask_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var taskRequest = new TaskRequest
            {
                Titulo = "Tarefa Teste",
                Descricao = "Descrição da tarefa",
                DataVencimento = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString(),
            };

            var taskEntity = new TaskEntity
            {
                Id = 1,
                Title = taskRequest.Titulo,
                Description = taskRequest.Descricao,
                DueDate = taskRequest.DataVencimento,
                Status = taskRequest.Status
            };

            var validationResult = Result.Success();
            var repositoryResult = Result<TaskEntity>.Success(taskEntity);

            _mockValidator.Setup(v => v.Validate(It.IsAny<TaskRequest>()))
                .Returns(validationResult);

            _mockRepository.Setup(r => r.Add(It.IsAny<TaskEntity>()))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.CreateTask(taskRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(taskRequest.Titulo, result.Data.Titulo);
            Assert.Equal(taskRequest.Descricao, result.Data.Descricao);

            _mockValidator.Verify(v => v.Validate(It.IsAny<TaskRequest>()), Times.Once);
            _mockRepository.Verify(r => r.Add(It.IsAny<TaskEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateTask_WithInvalidData_ShouldReturnValidationError()
        {
            // Arrange
            var taskRequest = new TaskRequest
            {
                Titulo = "", // Título inválido
                Descricao = "Descrição",
                DataVencimento = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString()
            };

            var validationResult = Result.Failure("Título é obrigatório");

            _mockValidator.Setup(v => v.Validate(It.IsAny<TaskRequest>()))
                .Returns(validationResult);

            // Act
            var result = await _taskService.CreateTask(taskRequest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Título é obrigatório", result.Error.Message);

            _mockValidator.Verify(v => v.Validate(It.IsAny<TaskRequest>()), Times.Once);
            _mockRepository.Verify(r => r.Add(It.IsAny<TaskEntity>()), Times.Never);
        }

        [Fact]
        public async Task CreateTask_WhenRepositoryFails_ShouldReturnError()
        {
            // Arrange
            var taskRequest = new TaskRequest
            {
                Titulo = "Tarefa Teste",
                Descricao = "Descrição",
                DataVencimento = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString(),
            };

            var validationResult = Result.Success();
            var expectedError = TaskErrors.Unexpected("Erro ao salvar no banco");
            var failureResult = Result.Failure<TaskEntity>(expectedError);

            _mockValidator.Setup(v => v.Validate(It.IsAny<TaskRequest>()))
                .Returns(validationResult);

            _mockRepository.Setup(r => r.Add(It.IsAny<TaskEntity>()))
                .ReturnsAsync(failureResult);

            // Act
            var result = await _taskService.CreateTask(taskRequest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro ao salvar no banco", result.Error.Message);

            _mockValidator.Verify(v => v.Validate(It.IsAny<TaskRequest>()), Times.Once);
            _mockRepository.Verify(r => r.Add(It.IsAny<TaskEntity>()), Times.Once);
        }

        #endregion

        #region GetTask Tests

        [Fact]
        public async Task GetTask_WithExistingId_ShouldReturnTask()
        {
            // Arrange
            int taskId = 1;
            var taskEntity = new TaskEntity
            {
                Id = taskId,
                Title = "Tarefa Existente",
                Description = "Descrição",
                DueDate = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString(),
            };

            var repositoryResult = Result<TaskEntity>.Success(taskEntity);

            _mockRepository.Setup(r => r.Get(taskId))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.GetTask(taskId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(taskId, result.Data.Id);
            Assert.Equal(taskEntity.Title, result.Data.Titulo);

            _mockRepository.Verify(r => r.Get(taskId), Times.Once);
        }

        [Fact]
        public async Task GetTask_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int taskId = 999;
            var repositoryResult = Result<TaskEntity>.Success<TaskEntity>(null!);

            _mockRepository.Setup(r => r.Get(taskId))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.GetTask(taskId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);

            _mockRepository.Verify(r => r.Get(taskId), Times.Once);
        }

        [Fact]
        public async Task GetTask_WhenRepositoryFails_ShouldReturnError()
        {
            // Arrange
            int taskId = 1;

            var error = TaskErrors.Unexpected("Erro de banco de dados");
            var repositoryResult = Result.Failure<TaskEntity>(error);

            _mockRepository.Setup(r => r.Get(taskId))
                .Returns(Task.FromResult<Result<TaskEntity>>(repositoryResult));

            // Act
            var result = await _taskService.GetTask(taskId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro de banco de dados", result.Error.Message);

            _mockRepository.Verify(r => r.Get(taskId), Times.Once);
        }

        #endregion

        #region UpdateTask Tests

        [Fact]
        public async Task UpdateTask_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            int taskId = 1;
            var taskRequest = new TaskRequest
            {
                Titulo = "Tarefa Atualizada",
                Descricao = "Nova descrição",
                DataVencimento = DateTime.Now.AddDays(14),
                Status = TaskStatusEnum.Pendente.ToString(),
            };

            var existingTask = new TaskEntity
            {
                Id = taskId,
                Title = "Tarefa Antiga",
                Description = "Descrição antiga",
                DueDate = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString()
            };

            var updatedTask = new TaskEntity
            {
                Id = taskId,
                Title = taskRequest.Titulo,
                Description = taskRequest.Descricao,
                DueDate = taskRequest.DataVencimento,
                Status = taskRequest.Status
            };

            var validationResult = Result.Success();
            var getResult = Result<TaskResponse>.Success(TaskMapper.MapToResponse(existingTask));
            var updateResult = Result<TaskEntity>.Success(updatedTask);

            _mockValidator.Setup(v => v.Validate(It.IsAny<TaskRequest>()))
                .Returns(validationResult);

            _mockRepository.Setup(r => r.Get(taskId))
                .Returns(Task.FromResult(Result<TaskEntity>.Success(existingTask)));

            _mockRepository.Setup(r => r.Update(taskId, It.IsAny<TaskEntity>()))
                .Returns(Task.FromResult(updateResult));

            // Act
            var result = await _taskService.UpdateTask(taskId, taskRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(taskId, result.Data.Id);
            Assert.Equal(taskRequest.Titulo, result.Data.Titulo);
            Assert.Equal(taskRequest.Descricao, result.Data.Descricao);
            Assert.Equal(taskRequest.Status, result.Data.Status);

            _mockValidator.Verify(v => v.Validate(It.IsAny<TaskRequest>()), Times.Once);
            _mockRepository.Verify(r => r.Get(taskId), Times.Once);
            _mockRepository.Verify(r => r.Update(taskId, It.IsAny<TaskEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTask_WithInvalidData_ShouldReturnValidationError()
        {
            // Arrange
            int taskId = 1;
            var taskRequest = new TaskRequest
            {
                Titulo = "", // Título inválido
                Descricao = "Descrição",
                DataVencimento = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString()
            };

            var validationResult = Result.Failure("Título é obrigatório");

            _mockValidator.Setup(v => v.Validate(It.IsAny<TaskRequest>()))
                .Returns(validationResult);

            // Act
            var result = await _taskService.UpdateTask(taskId, taskRequest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Título é obrigatório", result.Error.Message);

            _mockValidator.Verify(v => v.Validate(It.IsAny<TaskRequest>()), Times.Once);
            _mockRepository.Verify(r => r.Get(It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<TaskEntity>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTask_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int taskId = 999;
            var taskRequest = new TaskRequest
            {
                Titulo = "Tarefa Teste",
                Descricao = "Descrição",
                DataVencimento = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString()
            };

            var validationResult = Result.Success();

            _mockValidator.Setup(v => v.Validate(It.IsAny<TaskRequest>()))
                .Returns(validationResult);

            _mockRepository.Setup(r => r.Get(taskId))
                .Returns(Task.FromResult(Result<TaskEntity>.Success<TaskEntity>(null!)));

            // Act
            var result = await _taskService.UpdateTask(taskId, taskRequest);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);

            _mockValidator.Verify(v => v.Validate(It.IsAny<TaskRequest>()), Times.Once);
            _mockRepository.Verify(r => r.Get(taskId), Times.Once);
            _mockRepository.Verify(r => r.Update(It.IsAny<int>(), It.IsAny<TaskEntity>()), Times.Never);
        }

        #endregion

        #region DeleteTask Tests

        [Fact]
        public async Task DeleteTask_WithExistingId_ShouldReturnSuccess()
        {
            // Arrange
            int taskId = 1;
            var deletedTask = new TaskEntity
            {
                Id = taskId,
                Title = "Tarefa para deletar",
                Description = "Descrição",
                DueDate = DateTime.Now.AddDays(7),
                Status = TaskStatusEnum.Pendente.ToString()
            };

            var repositoryResult = Result<TaskEntity>.Success(deletedTask);

            _mockRepository.Setup(r => r.Delete(taskId))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(taskId, result.Data.Id);

            _mockRepository.Verify(r => r.Delete(taskId), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int taskId = 999;
            var repositoryResult = Result.Failure<TaskEntity>(TaskErrors.NotFound());

            _mockRepository.Setup(r => r.Delete(taskId))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);

            _mockRepository.Verify(r => r.Delete(taskId), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_WhenRepositoryFails_ShouldReturnError()
        {
            // Arrange
            int taskId = 1;
            var repositoryError = TaskErrors.Unexpected("Erro ao deletar no banco");
            var repositoryResult = Result.Failure<TaskEntity>(repositoryError);

            _mockRepository.Setup(r => r.Delete(taskId))
                .Returns(Task.FromResult<Result<TaskEntity>>(repositoryResult));

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro ao deletar no banco", result.Error.Message);

            _mockRepository.Verify(r => r.Delete(taskId), Times.Once);
        }

        #endregion

        #region ListTask Tests

        [Fact]
        public async Task ListTask_WithFilters_ShouldReturnFilteredList()
        {
            // Arrange
            string? status = "Pendente";
            DateTime? dueDate = DateTime.Now.AddDays(7);

            var tasks = new List<TaskEntity>
            {
                new TaskEntity { Id = 1, Title = "Tarefa 1", Status = TaskStatusEnum.Pendente.ToString(), DueDate = DateTime.Now.AddDays(5) },
                new TaskEntity { Id = 2, Title = "Tarefa 2", Status = TaskStatusEnum.Pendente.ToString(), DueDate = DateTime.Now.AddDays(7) }
            };

            var repositoryResult = Result.Success<IEnumerable<TaskEntity>>(tasks);

            _mockRepository.Setup(r => r.List(status, dueDate, 0, 0))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.ListTask(status, dueDate, 0, 0);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count());

            _mockRepository.Verify(r => r.List(status, dueDate, 0, 0), Times.Once);
        }

        [Fact]
        public async Task ListTask_WithNoResults_ShouldReturnNotFound()
        {
            // Arrange
            string? status = "Concluído";
            DateTime? dueDate = DateTime.Now.AddDays(30);

            var tasks = new List<TaskEntity>();
            var repositoryResult = Result.Success<IEnumerable<TaskEntity>>(tasks);

            _mockRepository.Setup(r => r.List(status, dueDate,0 ,0))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.ListTask(status, dueDate, 0, 0);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorType.NotFound, result.Error.Type);

            _mockRepository.Verify(r => r.List(status, dueDate, 0, 0), Times.Once);
        }

        [Fact]
        public async Task ListTask_WhenRepositoryFails_ShouldReturnError()
        {
            // Arrange
            string? status = null;
            DateTime? dueDate = null;

            var repositoryResult = Result.Failure<IEnumerable<TaskEntity>>("Erro ao listar");

            _mockRepository.Setup(r => r.List(status, dueDate, 0, 0))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.ListTask(status, dueDate, 0, 0);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Erro ao listar", result.Error.Message);

            _mockRepository.Verify(r => r.List(status, dueDate, 0, 0), Times.Once);
        }

        [Fact]
        public async Task ListTask_WithoutFilters_ShouldReturnAllTasks()
        {
            // Arrange
            string? status = null;
            DateTime? dueDate = null;

            var tasks = new List<TaskEntity>
            {
                new TaskEntity { Id = 1, Title = "Tarefa 1", Status = TaskStatusEnum.Pendente.ToString() },
                new TaskEntity { Id = 2, Title = "Tarefa 2", Status = TaskStatusEnum.EmProgresso.ToString() },
                new TaskEntity { Id = 3, Title = "Tarefa 3", Status = TaskStatusEnum.Concluida.ToString() }
            };

            var repositoryResult = Result.Success<IEnumerable<TaskEntity>>(tasks);

            _mockRepository.Setup(r => r.List(status, dueDate, 0, 0))
                .Returns(Task.FromResult(repositoryResult));

            // Act
            var result = await _taskService.ListTask(status, dueDate, 0, 0);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count());

            _mockRepository.Verify(r => r.List(status, dueDate, 0, 0), Times.Once);
        }

        #endregion
    }
}