(function ($) {
    app.modals.CreateOrEditUserReviewModal = function () {

        var _userReviewsService = abp.services.app.userReviews;

        var _modalManager;
        var _$userReviewInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userReviewInformationForm = _modalManager.getModal().find('form[name=UserReviewInformationsForm]');
            _$userReviewInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$userReviewInformationForm.valid()) {
                return;
            }

            var userReview = _$userReviewInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _userReviewsService.createOrEdit(
				userReview
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditUserReviewModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);