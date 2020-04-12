(function ($) {
    app.modals.CreateOrEditOrderModal = function () {

        var _ordersService = abp.services.app.orders;

        var _modalManager;
        var _$orderInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$orderInformationForm = _modalManager.getModal().find('form[name=OrderInformationsForm]');
            _$orderInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$orderInformationForm.valid()) {
                return;
            }

            var order = _$orderInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _ordersService.createOrEdit(
				order
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditOrderModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);