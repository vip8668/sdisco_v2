(function ($) {
    app.modals.CreateOrEditClaimReasonModal = function () {

        var _claimReasonsService = abp.services.app.claimReasons;

        var _modalManager;
        var _$claimReasonInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$claimReasonInformationForm = _modalManager.getModal().find('form[name=ClaimReasonInformationsForm]');
            _$claimReasonInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$claimReasonInformationForm.valid()) {
                return;
            }

            var claimReason = _$claimReasonInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _claimReasonsService.createOrEdit(
				claimReason
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditClaimReasonModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);