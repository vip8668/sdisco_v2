(function ($) {
    app.modals.CreateOrEditUserReviewDetailModal = function () {

        var _userReviewDetailsService = abp.services.app.userReviewDetails;

        var _modalManager;
        var _$userReviewDetailInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userReviewDetailInformationForm = _modalManager.getModal().find('form[name=UserReviewDetailInformationsForm]');
            _$userReviewDetailInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$userReviewDetailInformationForm.valid()) {
                return;
            }

            var userReviewDetail = _$userReviewDetailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _userReviewDetailsService.createOrEdit(
				userReviewDetail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditUserReviewDetailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);