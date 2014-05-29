/// <reference path="http://ajax.googleapis.com/ajax/libs/angularjs/1.3.0-beta.10/angular.js" />

angular.module('app', ['ngRoute'])

    .config(function ($routeProvider) {
        $routeProvider
            .when('/', { templateUrl: '/home' })
            .when('/about', { templateUrl: '/about' })
            .when('/page-not-found', { templateUrl: '/page-not-found' })
            .when('/login', { templateUrl: '/login', controller: 'LoginCtrl' })
            .otherwise({ redirectTo: '/page-not-found' });
    })

    .controller('MainCtrl', function ($scope, $rootScope, $location) {
        $scope.logout = function () {
            $rootScope.token = null;
            $location.path('/');
        };
    })

    .controller('LoginCtrl', function ($scope, $rootScope, $http, $location) {

        // Sample credentials
        $scope.username = 'user';
        $scope.password = 'Passw0rd';

        $scope.login = function (username, password) {
            $http({
                method: 'POST',
                url: '/token',
                data: 'grant_type=password&username=' + encodeURIComponent(username) +
                      '&password=' + encodeURIComponent(password),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $rootScope.token = data;
                $location.path('/');
            });
        };
    });