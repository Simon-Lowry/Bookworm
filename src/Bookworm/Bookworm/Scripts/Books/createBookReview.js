



$(function () {
	$('#form-createBookReview').on('submit',
        function (e) {

			var model = {
				Rating: $("#Rating").val(),
				UserId: $("#UserId").val(),
				BookId: $("#BookId").val(),
				Description: $("#Description").val()
            };

		    var postResponseSuccess = false;

		    $.ajax({
				type: "POST",
				url: "/Books/CreateBookReview",
				data: model,
				datatype: "html",
				success: function (data) {
				    postResponseSuccess = true;

                    if (postResponseSuccess == true) {
                        $('#msg-CreateBookReviewSuccess').fadeIn(1500).delay(1500).fadeOut(1500);
                        setTimeout(location.reload.bind(location), 4500);
				        
                    } else {
                        $('#msg-CreateBookReviewFail').fadeIn(1500).delay(1500).fadeOut(1500);
                        setTimeout(location.reload.bind(location), 4500);
				    }
				}
            });
		    $('#createBookReviewModal').modal('hide');
		    e.preventDefault();		    
		});

});


$(function () {
    $('#form-editBookReview').on('submit',
        function (e) {
            var model = {
                Rating: $("#Rating").val(),
                UserId: $("#UserId").val(),
                BookId: $("#BookId").val(),
                ReviewId: $("#ReviewId").val(),
                Description: $("#Description").val()
            };


            var postResponseSuccess = false;

            $.ajax({
                type: "POST",
                url: "/Books/UpdateBookReview",
                data: model,
                datatype: "html",
                success: function (data) {
                    postResponseSuccess = true;

                    if (postResponseSuccess == true) {
                        $('#msg-CreateBookReviewSuccess').fadeIn(1500).delay(1500).fadeOut(1500);
                        setTimeout(location.reload.bind(location), 4500);

                    } else {
                        $('#msg-CreateBookReviewFail').fadeIn(1500).delay(1500).fadeOut(1500);
                        setTimeout(location.reload.bind(location), 4500);
                    }
                }
            });
           
            $('#editBookReviewModal').modal('hide');
            e.preventDefault();

        });

});


$(function () {
    $('.openReadOnlyModal').on('click', function () {
        var title = $("#BookTitle").val();

        $('#readOnlyBookReviewModal').modal('show');
    });
});



$(function () {
    $('.closeButton').on('click', function () {
        $('#editBookReviewModal').modal('hide');
        $('#createBookReviewModal').modal('hide');
    });
});


function updateRating(value) {
    document.getElementById('Rating').value = value;
}
