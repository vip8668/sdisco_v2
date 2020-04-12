(function ($) {
    app.modals.CreateOrEditPartnerShipModal = function () {

        var _partnerShipsService = abp.services.app.partnerShips;

        var _modalManager;
        var _$partnerShipInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$partnerShipInformationForm = _modalManager.getModal().find('form[name=PartnerShipInformationsForm]');
            _$partnerShipInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$partnerShipInformationForm.valid()) {
                return;
            }

            var partnerShip = _$partnerShipInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _partnerShipsService.createOrEdit(
				partnerShip
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPartnerShipModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);