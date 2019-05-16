    var originalMoviesArray = [];

    $(document).ready(function () {
        boxvalue = document.getElementById('originalMoviesArr').value;
        elementsArr = boxvalue.split(',');
        originalMoviesArray = elementsArr.map(Number);
    });

    function removeMovieFromActorLoaded(event) {  
    
        var movieOld = event.target.children[0].innerHTML;
        var movieIdOld = Number(movieOld.split(' ')[0]); 

        for (var m = originalMoviesArray.length - 1; m >= 0; m--)
        {
            if (originalMoviesArray[m] == movieIdOld)
            {
                originalMoviesArray.splice(m, 1);
                break;
            }
        }

        $(document).on("click", ".deleteMovie-btn", function () {
            var clickedBtnOld = $(this);  
            clickedBtnOld.parent().remove();
            clickedBtnOld.remove();            
        });
      
    }    