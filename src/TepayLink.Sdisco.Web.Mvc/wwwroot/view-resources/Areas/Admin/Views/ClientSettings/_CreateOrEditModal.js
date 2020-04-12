(function ($) {
    app.modals.CreateOrEditClientSettingModal = function () {

        var _clientSettingsService = abp.services.app.clientSettings;

        var _modalManager;
        var _$clientSettingInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$clientSettingInformationForm = _modalManager.getModal().find('form[name=ClientSettingInformationsForm]');
            _$clientSettingInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$clientSettingInformationForm.valid()) {
                return;
            }

            var clientSetting = _$clientSettingInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _clientSettingsService.createOrEdit(
				clientSetting
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditClientSettingModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);