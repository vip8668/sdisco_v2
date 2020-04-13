(function ($) {
    app.modals.CreateOrEditTransactionModal = function () {

        var _transactionsService = abp.services.app.transactions;

        var _modalManager;
        var _$transactionInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$transactionInformationForm = _modalManager.getModal().find('form[name=TransactionInformationsForm]');
            _$transactionInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$transactionInformationForm.valid()) {
                return;
            }

            var transaction = _$transactionInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _transactionsService.createOrEdit(
				transaction
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditTransactionModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);