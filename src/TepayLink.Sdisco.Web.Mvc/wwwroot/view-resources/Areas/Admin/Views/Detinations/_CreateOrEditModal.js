(function ($) {
    app.modals.CreateOrEditDetinationModal = function () {

        var _detinationsService = abp.services.app.detinations;

        var _modalManager;
        var _$detinationInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$detinationInformationForm = _modalManager.getModal().find('form[name=DetinationInformationsForm]');
            _$detinationInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$detinationInformationForm.valid()) {
                return;
            }

            var detination = _$detinationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _detinationsService.createOrEdit(
				detination
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDetinationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);