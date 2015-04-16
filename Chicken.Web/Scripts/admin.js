angular.module('app', [])
  .controller('AdminController', ['$scope', '$http', function ($scope, $http) {
      $scope.totalCount = 0;
      $scope.spamCount = 0;
      $scope.posts = [];
      $scope.isLoading = false;
      $scope.isUpdating = false;
      
      $scope.$watchCollection('posts', function (news, olds) {
          debugger;
      });
      
      $scope.init = function (totalCount, spamCount) {
          $scope.totalCount = totalCount;
          $scope.spamCount = spamCount;
      };

      $scope.getPosts = function () {
          if (!$scope.isLoading) {
              $scope.isLoading = true;
              $http.get('admin/getposts?skip=' + $scope.posts.length).success(function (data) {
                  for (var i = 0; i < data.length; i++) {
                      $scope.posts.push(data[i]);
                      $scope.isLoading = false;
                  }
              });
          }
      };

      $scope.loadNewPosts = function () {
          $scope.isUpdating = true;
          $http.get('admin/update').success(function (data) {
              for (var i = 0; i < data.length; i++) {
                  $scope.posts.unshift(data[i]);
                  $scope.isUpdating = false;
              }
          });
      };

      $scope.getPosts();
  }]);
