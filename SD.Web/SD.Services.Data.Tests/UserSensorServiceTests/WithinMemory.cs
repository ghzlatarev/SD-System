using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Exceptions;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using SD.Services.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.UserSensorServiceTests
{

    [TestClass]
    public class WithInMemory
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetSensorByIdAsync_ShouldThrowArgumentNullExceptionsWhenUserSensorIsNull()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetSensorByIdAsync_ShouldThrowArgumentNullExceptionsWhenUserSensorIsNull")
                .Options;

            var result = new UserSensor();

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                result = await SUT.GetSensorByIdAsync("82a231b1-ea5d-4356-8266-b6b42471653e");
            }
        }

        [TestMethod]
        public async Task GetSensorByIdAsync_ShouldReturnValidSensor()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "GetSensorByIdAsync_ShouldReturnValidSensor")
                .Options;

            UserSensor userSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b1-ea5d-4356-8266-b6b42471653e",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };

            var result = new UserSensor();

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                await actContext.UserSensors.AddAsync(userSensor);
                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                result = await SUT.GetSensorByIdAsync("82a2e1b1-ea5d-4356-8266-b6b42471653e");
            }

            //Assert
            using (DataContext assertContext = new DataContext(contextOptions))
            {
                Assert.IsTrue(assertContext.UserSensors.Any(s => s.Coordinates.Equals(result.Coordinates)));
                Assert.IsTrue(assertContext.UserSensors.Any(s => s.LastValueUser.Equals(result.LastValueUser)));
                Assert.IsTrue(assertContext.UserSensors.Any(s => s.SensorId.Equals(result.SensorId)));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AddUserSensorAsync_ShouldThrowArgumentNullExceptionsWhenSensorNameIsIsNull()
        {
            UserSensor userSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b1-ea5d-4356-8266-b6b42471653e",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = null,
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };

            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "AddUserSensorAsync_ShouldThrowArgumentNullExceptionsWhenSensorNameIsIsNull")
                .Options;

            var result = new UserSensor();

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                await SUT.AddUserSensorAsync(userSensor.Id, userSensor.SensorId, userSensor.Name, userSensor.Description, userSensor.Latitude,
                    userSensor.Longitude, userSensor.AlarmMin, userSensor.AlarmMax, userSensor.PollingInterval, userSensor.AlarmTriggered,
                    userSensor.IsPublic, userSensor.LastValueUser, userSensor.Type);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(EntityAlreadyExistsException))]
        public async Task AddUserSensorAsync_ShouldThrowEntityAlreadyExistsExceptionExceptionsWhenSensorExists()
        {
            UserSensor validUserSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b1-ea5d-4356-8266-b6b42471653e",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "name example",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44,
                Sensor = new Sensor {
                     Id = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                     SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e"
                }

            };

            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "AddUserSensorAsync_ShouldThrowEntityAlreadyExistsExceptionExceptionsWhenSensorExists")
                .Options;

            var result = new UserSensor();

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                await actContext.UserSensors.AddAsync(validUserSensor);
                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                await SUT.AddUserSensorAsync(validUserSensor.Id, validUserSensor.SensorId, validUserSensor.Name, validUserSensor.Description, validUserSensor.Latitude,
                    validUserSensor.Longitude, validUserSensor.AlarmMin, validUserSensor.AlarmMax, validUserSensor.PollingInterval, validUserSensor.AlarmTriggered,
                    validUserSensor.IsPublic, validUserSensor.LastValueUser, validUserSensor.Type);

            }
        }

        [TestMethod]
        public async Task AddUserSensorAsync_ShouldAddAValidSensorToTheDataBase()
        {
            UserSensor validUserSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "name example",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44,
                Sensor = new Sensor
                {
                    Id = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                    SensorId = "81a2e1b1-ea5d-4356-8266-b6b42481653e"
                }

            };

            UserSensor userSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "name sensor",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653f",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44,
                Sensor = new Sensor
                {
                    Id = "81a2e1b1-ea5d-4356-8266-b6b42471663f",
                    SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653f"
                }

            };

            Sensor sensorApi = new Sensor
            {
                Id = "81a2e1b1-ea5d-4356-8266-b6b42471653f",
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653f"
            };

            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "AddUserSensorAsync_ShouldAddAValidSensorToTheDataBase")
                .Options;

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                await actContext.Sensors.AddAsync(sensorApi);
                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                await SUT.AddUserSensorAsync(userSensor.Id, userSensor.SensorId, userSensor.Name, userSensor.Description, userSensor.Latitude,
                    userSensor.Longitude, userSensor.AlarmMin, userSensor.AlarmMax, userSensor.PollingInterval, userSensor.AlarmTriggered,
                    userSensor.IsPublic, userSensor.LastValueUser, userSensor.Type);

                var result = new List<UserSensor>();
                result.Add(actContext.UserSensors.FirstOrDefault(s => s.Name == userSensor.Name));
                //Assert
                using (DataContext assertContext = new DataContext(contextOptions))
                {
                    Assert.IsTrue(result[0].Name == userSensor.Name);
                    Assert.IsTrue(result.Count == 1);
                    Assert.IsTrue(result[0].SensorId == userSensor.SensorId);
                }
            }
        }

        [TestMethod]
        public async Task UpdateUserSensorAsync_ShouldUpdateSensorInDataBase()
        {
            UserSensor userSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };
            UserSensor userSensorUpdated = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "updated",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };

            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserSensorAsync_ShouldUpdateSensorInDataBase")
                .Options;


            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();
                await actContext.AddAsync(userSensor);
                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                await SUT.UpdateUserSensorAsync(userSensorUpdated);

            }

            using (DataContext assertContext = new DataContext(contextOptions))
            {
                var result = assertContext.UserSensors.ToList();
                Assert.IsTrue(result[1].Name == "updated");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task UpdateUserSensorAsync_ShouldThrowArgumentNullExceptionsWhenUserSensorIsIsNull()
        {


            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "UpdateUserSensorAsync_ShouldThrowArgumentNullExceptionsWhenUserSensorIsIsNull")
                .Options;

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                await SUT.UpdateUserSensorAsync(null);
            }
        }

        [TestMethod]
        public async Task ListPublicSensorsAsync_ShouldListPublicSensors()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ListPublicSensorsAsync_ShouldListPublicSensors")
                .Options;

            UserSensor userSensorPublic = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b1-ea5d-4356-8266-b6b42471653c",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp public",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };
            UserSensor userSensorPrivate = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b2-ea5d-4356-8266-b6b42471653q",
                IsPublic = false,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp private",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b3-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b4-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };
            UserSensor userSensorDeleted = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b2-ea5d-4356-8266-b6b42471653y",
                IsPublic = false,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp deleted",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b3-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b4-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };


            using (DataContext actContext = new DataContext(contextOptions))
            {
                await actContext.UserSensors.AddAsync(userSensorPublic);
                await actContext.UserSensors.AddAsync(userSensorPrivate);
                await actContext.UserSensors.AddAsync(userSensorDeleted);
                await actContext.SaveChangesAsync();

            }

            using (DataContext assertContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();
                UserSensorService SUT = new UserSensorService(assertContext, sensor.Object);
                var sensors = SUT.ListPublicSensorsAsync().Result;

                Assert.IsTrue(sensors.ToList().Count == 1);
            }
        }
        [TestMethod]
        public async Task ListSensorsForUserAsync_ShouldListPublicSensors()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ListSensorsForUserAsync_ShouldListSensorsForUser")
                .Options;

            UserSensor userSensorPublic = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b1-ea5d-4356-8266-b6b42471653c",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp public",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };
            UserSensor userSensorPrivate = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b2-ea5d-4356-8266-b6b42471653q",
                IsPublic = false,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp private",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b3-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665h",
                UserInterval = 44
            };
            UserSensor userSensorDeleted = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b2-ea5d-4356-8266-b6b42471653y",
                IsPublic = false,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp deleted",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b3-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b4-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };


            using (DataContext actContext = new DataContext(contextOptions))
            {
                await actContext.UserSensors.AddAsync(userSensorPublic);
                await actContext.UserSensors.AddAsync(userSensorPrivate);
                await actContext.UserSensors.AddAsync(userSensorDeleted);
                await actContext.SaveChangesAsync();

            }

            using (DataContext assertContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();
                UserSensorService SUT = new UserSensorService(assertContext, sensor.Object);
                var sensors = SUT.ListSensorsForUserAsync("81a2e1b1-ea5d-4356-8266-b6b42471665e").Result;

                Assert.IsTrue(sensors.ToList().Count == 1);
            }
        }

        [TestMethod]
        public async Task ListSensorByIdAsync_ShouldReturnValidSensor()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ListSensorByIdAsync_ShouldReturnValidSensor")
                .Options;

            UserSensor userSensor = new UserSensor
            {
                AlarmMin = 20,
                AlarmMax = 30,
                AlarmTriggered = true,
                Coordinates = "42.672143,23.292216",
                CreatedOn = DateTime.Now,
                Description = "Description122",
                Id = "82a2e1b1-ea5d-4356-8266-b6b42471653e",
                IsPublic = true,
                LastValueUser = "23",
                Latitude = "42.672143",
                Longitude = "23.292216",
                Name = "Thermostat temp",
                IsDeleted = false,
                PollingInterval = 50,
                SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
                TimeStamp = DateTime.Now,
                Type = "Celsius",
                UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
                UserInterval = 44
            };

            var result = new UserSensor();

            // Act
            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<ISensorService> sensor = new Mock<ISensorService>();

                await actContext.UserSensors.AddAsync(userSensor);
                await actContext.SaveChangesAsync();

                UserSensorService SUT = new UserSensorService(actContext, sensor.Object);

                result = await SUT.ListSensorByIdAsync("82a2e1b1-ea5d-4356-8266-b6b42471653e");
            }

            //Assert
            using (DataContext assertContext = new DataContext(contextOptions))
            {
                Assert.IsTrue(assertContext.UserSensors.Any(s => s.Coordinates.Equals(result.Coordinates)));
                Assert.IsTrue(assertContext.UserSensors.Any(s => s.LastValueUser.Equals(result.LastValueUser)));
                Assert.IsTrue(assertContext.UserSensors.Any(s => s.SensorId.Equals(result.SensorId)));
            }
        }

		[TestMethod]
		public async Task DisableUserSensor_ShouldFlagUserSensorAsDeleted_WhenPassedValidParameters()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "DisableUserSensor_ShouldFlagUserSensorAsDeleted_WhenPassedValidParameters")
				.Options;

			Guid Id = Guid.NewGuid();
			bool validIsDeleted = true;

			UserSensor validUserSensor = new UserSensor
			{
				Id = Id.ToString(),
				Name = "test"
			};

			UserSensor result = null;

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<ISensorService> sensorServicetMock = new Mock<ISensorService>();

				await actContext.AddAsync(validUserSensor);
				await actContext.SaveChangesAsync();

				UserSensorService SUT = new UserSensorService(
					actContext,
					sensorServicetMock.Object);

				result = await SUT.DisableUserSensor(Id.ToString());
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				Assert.IsNotNull(result);
				Assert.IsNotNull(result.DeletedOn);
				Assert.IsTrue(result.IsDeleted.Equals(validIsDeleted));
			}
		}

		[TestMethod]
		public async Task RestoreUserSensor_ShouldFlagChampionAsNotDeleted_WhenPassedValidParameters()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "RestoreUserSensor_ShouldFlagChampionAsNotDeleted_WhenPassedValidParameters")
				.Options;

			Guid Id = Guid.NewGuid();
			bool validIsDeleted = false;

			UserSensor validUserSensor = new UserSensor
			{
				Id = Id.ToString(),
				Name = "testChamp",
				DeletedOn = DateTime.UtcNow.AddHours(2),
				IsDeleted = true
			};

			UserSensor result = null;

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();

				await actContext.AddAsync(validUserSensor);
				await actContext.SaveChangesAsync();

				UserSensorService SUT = new UserSensorService(
					actContext,
					sensorServiceMock.Object);

				result = await SUT.RestoreUserSensor(Id.ToString());
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				Assert.IsNotNull(result);
				Assert.IsNull(result.DeletedOn);
				Assert.IsTrue(result.IsDeleted.Equals(validIsDeleted));
			}
		}

		[TestMethod]
		public async Task FilterUserSensorsAsync_ShouldReturnUserSensor_WhenPassedValidParameters()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "FilterUserSensorsAsync_ShouldReturnUserSensor_WhenPassedValidParameters")
				.Options;


			string validFilter = string.Empty;
			int validPageSize = 10;
			int validPageNumber = 1;

			UserSensor validUserSensor = new UserSensor
			{
				AlarmMin = 20,
				AlarmMax = 30,
				AlarmTriggered = true,
				Coordinates = "42.672143,23.292216",
				CreatedOn = DateTime.Now,
				Description = "Description122",
				Id = "82a2e1b1-ea5d-4356-8266-b6b42471653e",
				IsPublic = true,
				LastValueUser = "23",
				Latitude = "42.672143",
				Longitude = "23.292216",
				Name = "Thermostat temp",
				IsDeleted = false,
				PollingInterval = 50,
				SensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e",
				TimeStamp = DateTime.Now,
				Type = "Celsius",
				UserId = "81a2e1b1-ea5d-4356-8266-b6b42471665e",
				UserInterval = 44
			};

			IEnumerable<UserSensor> result = new List<UserSensor>();

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();

				await actContext.UserSensors.AddAsync(validUserSensor);
				await actContext.SaveChangesAsync();

				UserSensorService SUT = new UserSensorService(
					actContext,
					sensorServiceMock.Object);

				result = await SUT.FilterUserSensorsAsync(validFilter, validPageNumber, validPageSize);
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				var sensor = result.ToArray()[0];

				Assert.IsTrue(assertContext.UserSensors.Count().Equals(result.Count()));
				Assert.IsTrue(assertContext.UserSensors.Any(c => c.Name.Equals(sensor.Name)));
				Assert.IsTrue(assertContext.UserSensors.Any(c => c.Description.Equals(sensor.Description)));
				Assert.IsTrue(assertContext.UserSensors.Any(c => c.Type.Equals(sensor.Type)));
			}
		}
	}
}
