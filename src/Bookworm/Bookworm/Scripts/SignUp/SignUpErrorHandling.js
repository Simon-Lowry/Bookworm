



$(function () {
	$('#form-createBookReview').on('submit',
		function(e) {
			var rating = $("#Rating").val();
			var userId = $("#UserId").val();
			var bookId = $("#BookId").val();
			var description = $("#Description").val();

			var model = {
				Rating: rating,
				UserId: userId,
				BookId: bookId,
				Description: description
			};

			$.ajax({
				type: "POST",
				data: JSON.stringify(model),
				url: "/Books/CreateBookReview",
				contentType: "application/js"


			});
		});
});

