angular.module('app', [])
  .controller('AdminController', ['$scope', '$http', function ($scope, $http) {
      $scope.totalCount = 0;
      $scope.spamCount = 0;
      $scope.posts = [];
      $scope.modifiedPosts = [];
      $scope.isLoading = false;
      $scope.isUpdating = false;

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
                  }
				  $scope.isLoading = false;
              });
          }
      };

      $scope.changePost = function (el) {
          if (!$scope.modifiedPosts.contains(el)) {
              $scope.modifiedPosts.push(el);
          }
      };

      $scope.save = function () {
          var modifiedPosts = $scope.modifiedPosts;
          $http.post('admin/save', modifiedPosts).success(function () {
              alert('Изменения сохранены.');
          });
      };

      $scope.loadNewPosts = function () {
          $scope.isUpdating = true;
          $http.get('admin/update').success(function (data) {
              for (var i = 0; i < data.length; i++) {
                  $scope.posts.unshift(data[i]);                  
              }
			  $scope.isUpdating = false;
          });
      };

      $scope.getPosts();
  }]);
