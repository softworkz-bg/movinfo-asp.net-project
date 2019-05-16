var categoryArray = [];
var ignoredCategories = [];


$(function searchForCategories() {
    $("#CategorySearchInput").keyup(function (e) {
        if (event.which >= 65 && event.which <= 90) {
            var categoryName = $("#categoryName").val();
            ignoredCategories = categoryArray.join(',');
            $.post("/Category/FindCategories?categoryName=" + categoryName, "ignoredCategories=" + ignoredCategories, function (r) {
                //update ui with results
                $("#categoriesResultsTable").html(r);
            });
        }
    });
});

function addCategory(event) {
    var category = event.target.innerHTML;
    var categoryyId = category.split(' ')[0];

    categoryArray.push(categoryyId);

    d = document.createElement('div');
    $(d).addClass("categoriesAdded")
        .html(category)
        .appendTo($("#placedCategoryResults"))
        .click(function removeCat() {
            var currCat = $(this).html();
            var currCatId = currCat.split(' ')[0];
            removeA(categoryArray, currCatId);
            removeA(ignoredCategories, currCatId);
            $(this).remove();
        });

    $(this).remove();

    $(document).on("click", ".btn-category", function () {
        var clickedBtn = $(this);
        clickedBtn.remove();
    });
}

function addAllCategoriesEdited(event) {	
    var resultedCategoriesOldAndNew = categoryArray.join(',') + ',' + originalCategoriesArray.join(',');
    var resultedCategoriesOldAndNewTrimmed = resultedCategoriesOldAndNew.replace(/(^,)|(,$)/g, "");
    $(".categories-form-submit").val(resultedCategoriesOldAndNewTrimmed);		
}
