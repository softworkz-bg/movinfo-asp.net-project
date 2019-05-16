 $(document).on('click', '#prettyButton', function () {
            $("#uglyButton").click();            
        });

$(document).ready(function () {
            $('input[type="file"]').change(function (e) {
                var fileName = e.target.files[0].name;
                $("#resultOfButton").append("File Selected: " + fileName);
            });
});