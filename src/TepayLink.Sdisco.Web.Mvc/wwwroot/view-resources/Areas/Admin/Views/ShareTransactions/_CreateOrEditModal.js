(function ($) {
    app.modals.CreateOrEditShareTransactionModal = function () {

        var _shareTransactionsService = abp.services.app.shareTransactions;

        var _modalManager;
        var _$shareTransactionInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$shareTransactionInformationForm = _modalManager.getModal().find('form[name=ShareTransactionInformationsForm]');
            _$shareTransactionInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$shareTransactionInformationForm.valid()) {
                return;
            }

            var shareTransaction = _$shareTransactionInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _shareTransactionsService.createOrEdit(
				shareTransaction
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditShareTransactionModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);