    var originalCategoriesArray = [];

    $(document).ready(function () {
        catBoxvalue = document.getElementById('originalCategoriesArr').value;
        catElementsArr = catBoxvalue.split(',');
        originalCategoriesArray = catElementsArr.map(Number);
    });

    function removeCategoryFromMovieLoaded(event) {  
        var categoryOld = event.target.children[0].innerHTML;
        var categoryIdOld = Number(categoryOld.split(' ')[0]); 

        console.log(categoryOld);

        for (var c = originalCategoriesArray.length - 1; c >= 0; c--)
        {
            if (originalCategoriesArray[c] == categoryIdOld)
            {
                originalCategoriesArray.splice(c, 1);
                break;
            }
        }

        $(document).on("click", ".deleteCategory-btn", function () {
            var clickedBtnCatOld = $(this);
            clickedBtnCatOld.parent().remove();
            clickedBtnCatOld.remove();
        });
    }    