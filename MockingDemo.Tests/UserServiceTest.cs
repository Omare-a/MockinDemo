using MockinDemo;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockingDemo.Tests;

public class UserServiceTest
{
    [Fact]
    public void CountActiveUser_ReturnsNumberOfActiveUser()
    {
        //Arrange: fejkdata (som annars hade kommit från DB/API)
        //var user = new MockinDemo.User();
        var fakeUser = new List<User>
        {
            new User {Name = "Adam", IsActive = true},
            new User {Name = "Bilal", IsActive=false},
            new User {Name = "Charlie", IsActive=true},
        };

        //Arange: skapar mock av beroendet
        var repoMock = new Mock<IUserRepository>();

        //när någon anropar getUser(), returnera vår fejklista
        repoMock.Setup(r => r.GetUsers()).Returns(fakeUser);

        // injecta mocken i service-klassen
        var service = new UserService(repoMock.Object);

        //act
        var result = service.CountActiveUsers();

        // assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void CountActiveUsers_CallRepositoryExactlyOnce()
    {
        //arrange
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetUsers()).Returns(new List<User>());

        var service = new UserService(repoMock.Object);

        //act
        service.CountActiveUsers();

        //Assert: verifiera att GetUsers anropades exakt 1 gång
        repoMock.Verify(r => r.GetUsers(), Times.Once);
    }

    [Fact]
    public void CountActiveUser_Throw_WhenRepositoryReturnsNull()
    {
        //arrange
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetUsers()).Returns((List<User>)null!);

        var service = new UserService(repoMock.Object);

        //act + assert
        Assert.Throws<InvalidOperationException>(() => service.CountActiveUsers());
           
    }
}
