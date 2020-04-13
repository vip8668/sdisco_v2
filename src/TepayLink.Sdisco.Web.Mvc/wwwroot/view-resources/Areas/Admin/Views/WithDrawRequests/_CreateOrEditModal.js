(function ($) {
    app.modals.CreateOrEditWithDrawRequestModal = function () {

        var _withDrawRequestsService = abp.services.app.withDrawRequests;

        var _modalManager;
        var _$withDrawRequestInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$withDrawRequestInformationForm = _modalManager.getModal().find('form[name=WithDrawRequestInformationsForm]');
            _$withDrawRequestInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$withDrawRequestInformationForm.valid()) {
                return;
            }

            var withDrawRequest = _$withDrawRequestInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _withDrawRequestsService.createOrEdit(
				withDrawRequest
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditWithDrawRequestModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);