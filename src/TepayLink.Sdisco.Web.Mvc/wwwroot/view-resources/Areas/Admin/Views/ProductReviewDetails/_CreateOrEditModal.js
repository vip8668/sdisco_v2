(function ($) {
    app.modals.CreateOrEditProductReviewDetailModal = function () {

        var _productReviewDetailsService = abp.services.app.productReviewDetails;

        var _modalManager;
        var _$productReviewDetailInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productReviewDetailInformationForm = _modalManager.getModal().find('form[name=ProductReviewDetailInformationsForm]');
            _$productReviewDetailInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$productReviewDetailInformationForm.valid()) {
                return;
            }

            var productReviewDetail = _$productReviewDetailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productReviewDetailsService.createOrEdit(
				productReviewDetail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductReviewDetailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);