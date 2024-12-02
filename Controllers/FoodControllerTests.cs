using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FoodReggie_1.Controllers;
using FoodReggie_1.DAL;
using FoodReggie_1.Models;
using FoodReggie_1.ViewModels;

namespace FoodReggie_1.Test.Controllers;

public class FoodControllerTests{
    [Fact]
    public async Task TestTableOK(){
        var foodList = new List<Food>(){
            new Food{
                FoodId = 1,
                Name = "Chicken",
                FoodGroup = "Meat",
                Calories = 111,
                Protein = 23,
                Carbohydrates = 0,
                Fats = 2.1,
                ImageURL = "/images/chicken.jpg"
            },
            new Food{
                FoodId = 2,
                Name = "Salmon Fillet",
                FoodGroup = "Meat",
                Calories = 224,
                Protein = 20,
                Carbohydrates = 0,
                Fats = 16,
                ImageURL = "/images/salmon.jpg"
            }
        };
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.GetAll()).ReturnsAsync(foodList);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.Table();

        var viewResult = Assert.IsType<ViewResult>(result);
        var foodsViewModel = Assert.IsAssignableFrom<FoodViewModel>(viewResult.ViewData.Model);
        Assert.Equal(2, foodsViewModel.Foods.Count());
        Assert.Equal(foodList, foodsViewModel.Foods);
    }

    [Fact]
    public async Task TestTableEmpty(){
        var emptyFoodList = new List<Food>();
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.GetAll()).ReturnsAsync(emptyFoodList);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.Table();

        var viewResult = Assert.IsType<ViewResult>(result);
        var foodsViewModel = Assert.IsAssignableFrom<FoodViewModel>(viewResult.ViewData.Model);
        Assert.Empty(foodsViewModel.Foods);
    }

    [Fact]
    public async Task TestCreateOK(){
        var testFood = new Food{
            Name = "Milk",
            FoodId = 1,
            FoodGroup = "Dairy",
            Calories = 123,
            Protein = 23,
            Carbohydrates = 32,
            Fats = 5.6,
            ImageURL = "/images/chicken.jpg"
        };
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.Create(testFood)).ReturnsAsync(true);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.Create(testFood);

        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", viewResult.ActionName);
    }

    [Fact]
    public async Task TestCreateNotOk(){
        var testFood = new Food{
            FoodId = 1,
            FoodGroup = "Dairy",
            Calories = 123,
            Protein = 23,
            Carbohydrates = 32,
            Fats = 5.6,
            ImageURL = "/images/chicken.jpg"
        };
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.Create(testFood)).ReturnsAsync(false);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.Create(testFood);

        var viewResult = Assert.IsType<ViewResult>(result);
        var viewFood = Assert.IsAssignableFrom<Food>(viewResult.ViewData.Model);
        Assert.Equal(testFood, viewFood); 
    }   

    [Fact]
    public async Task UpdateOk(){
        var testFood = new Food{
            FoodId = 1,
            Name = "Chicken",
            FoodGroup = "Meat",
            Calories = 111,
            Protein = 23,
            Carbohydrates = 9,
            Fats = 2.1,
            ImageURL = "/images/chicken.jpg"
        };
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.Update(testFood)).ReturnsAsync(true);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.Update(testFood);

        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", viewResult.ActionName);
    }

    [Fact]
    public async Task UpdateNotOk(){
        var testFood = new Food{
            FoodId = 1,
            Name = "Chicken",
            FoodGroup = "Meat",
            Calories = 111,
            Protein = 23,
            Carbohydrates = 9,
            Fats = 2.1,
            ImageURL = "/images/chicken.jpg"
        };
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.Update(testFood)).ReturnsAsync(false);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.Update(testFood);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Food>(viewResult.ViewData.Model);
        Assert.Equal(testFood, model);
    }

    [Fact]
    public async Task DeleteOK(){
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.Delete(1)).ReturnsAsync(true);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.ConfirmDelete(1);

        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", viewResult.ActionName);
    }

    [Fact]
    public async Task DeleteNotOK(){
        var mockFoodRepository = new Mock<IFoodRepository>();
        mockFoodRepository.Setup(repo => repo.Delete(1)).ReturnsAsync(false);
        var mockLogger = new Mock<ILogger<FoodController>>();
        var foodController = new FoodController(mockFoodRepository.Object, mockLogger.Object);

        var result = await foodController.ConfirmDelete(1);

        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Table", viewResult.ActionName);
    }
}