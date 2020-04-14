(function ($) {
    app.modals.CreateOrEditRefundReasonModal = function () {

        var _refundReasonsService = abp.services.app.refundReasons;

        var _modalManager;
        var _$refundReasonInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$refundReasonInformationForm = _modalManager.getModal().find('form[name=RefundReasonInformationsForm]');
            _$refundReasonInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$refundReasonInformationForm.valid()) {
                return;
            }

            var refundReason = _$refundReasonInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _refundReasonsService.createOrEdit(
				refundReason
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRefundReasonModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);