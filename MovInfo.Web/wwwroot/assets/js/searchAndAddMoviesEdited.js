var moviesArray = [];
var ignoredMovies = [];

$(function searchForMovies() {
    $("#MovieSearchInput").keyup(function (e) {
        if (event.which >= 65 && event.which <= 90) {
            var movieName = $("#movieName").val();
            ignoredMovies = moviesArray.join(',');
            $.post("/Movie/FindMovies?movieName=" + movieName, "ignoredMovies=" + ignoredMovies, function (r) {
                //update ui with results
                $("#resultsMoviesTable").html(r);
            });
        }
    });
});

function addMovie(event) {
    var movie = event.target.innerHTML;
    var movieId = movie.split(' ')[0];

    moviesArray.push(movieId);

    d = document.createElement('div');
    $(d).addClass("moviesAdded")
        .html(movie)
        .appendTo($("#placedMovieResults"))
        .click(function removeMovie() {
            var currMovie = $(this).html();
            var currMovieId = currMovie.split(' ')[0];
            removeA(moviesArray, currMovieId);
            removeA(ignoredMovies, currMovieId);
            $(this).remove();
        });

    $(this).remove();

    $(document).on("click", ".btn-movie", function () {
        var clickedMovBtn = $(this);
        clickedMovBtn.remove();
    });
}

function addAllMoviesEdited(event) {
    var resultedMoviesOldAndNew = moviesArray.join(',') + ',' + originalMoviesArray.join(',');
    var resultedMoviesOldAndNewTrimmed = resultedMoviesOldAndNew.replace(/(^,)|(,$)/g, "");
    $(".actors-form-submit").val(resultedMoviesOldAndNewTrimmed);
}

  