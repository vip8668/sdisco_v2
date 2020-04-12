(function ($) {
    app.modals.CreateOrEditUserSubcriberModal = function () {

        var _userSubcribersService = abp.services.app.userSubcribers;

        var _modalManager;
        var _$userSubcriberInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userSubcriberInformationForm = _modalManager.getModal().find('form[name=UserSubcriberInformationsForm]');
            _$userSubcriberInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$userSubcriberInformationForm.valid()) {
                return;
            }

            var userSubcriber = _$userSubcriberInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _userSubcribersService.createOrEdit(
				userSubcriber
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditUserSubcriberModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);