var vm = {
    movieIds: []
};

$(document).ready(function () {
    
    
    var customers = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/api/customers?query=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#customers').typeahead({
        minLength: 3,
        highlight: true
    }, {
        name: 'customers',
        display: 'name',
        source: customers
    }).on('typeahead:select', function(e, customer) {
        vm.customerId = customer.id;
    });

    var movies = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: '/api/movies?query=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#txtmovies').typeahead({
        minLength: 3,
        highlight: true
    }, {
        name: 'movies',
        display: 'name',
        source: movies
    }).on('typeahead:select', function (e, movie) {
        $("#movies").append("<li class='list-group-item'>" + movie.name + "</li>");

        $("#txtmovies").typeahead("val", "");
        vm.movieIds.push(movie.id);
    });

    $.validator.addMethod("validCustomer", function () {
        return vm.customerId && vm.customerId !== 0;
    }, "Please select a valid customer.");

    $.validator.addMethod("atLeastOneMovie", function () {
        return vm.movieIds.length > 0;
    }, "Please select at least one movie.");

    var validator = $("#newRental").validate({
        submitHandler: function() {
            $.ajax({
                    url: "/api/newRentals",
                    method: "post",
                    data: vm
                })
                .done(function() {
                    toastr.success("Rentals successfully recorded.");

                    $("#customers").typeahead("val", "");
                    $("#txtmovies").typeahead("val", "");
                    $("#movies").empty();

                    vm = { movieIds: [] };

                    validator.resetForm(); //reset the form in terms of its validation
                })
                .fail(function() {
                    toastr.error("Something unexpected happened.");
                });

            return false; //prevents the form to be submitted normally to the server
        }
    });
});