(function ($) {
    app.modals.CreateOrEditBankBranchModal = function () {

        var _bankBranchsService = abp.services.app.bankBranchs;

        var _modalManager;
        var _$bankBranchInformationForm = null;

		        var _BankBranchbankLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankBranchs/BankLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BankBranchs/_BankBranchBankLookupTableModal.js',
            modalClass: 'BankLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bankBranchInformationForm = _modalManager.getModal().find('form[name=BankBranchInformationsForm]');
            _$bankBranchInformationForm.validate();
        };

		          $('#OpenBankLookupTableButton').click(function () {

            var bankBranch = _$bankBranchInformationForm.serializeFormToObject();

            _BankBranchbankLookupTableModal.open({ id: bankBranch.bankId, displayName: bankBranch.bankBankName }, function (data) {
                _$bankBranchInformationForm.find('input[name=bankBankName]').val(data.displayName); 
                _$bankBranchInformationForm.find('input[name=bankId]').val(data.id); 
            });
        });
		
		$('#ClearBankBankNameButton').click(function () {
                _$bankBranchInformationForm.find('input[name=bankBankName]').val(''); 
                _$bankBranchInformationForm.find('input[name=bankId]').val(''); 
        });
		


        this.save = function () {
            if (!_$bankBranchInformationForm.valid()) {
                return;
            }
            if ($('#BankBranch_BankId').prop('required') && $('#BankBranch_BankId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Bank')));
                return;
            }

            var bankBranch = _$bankBranchInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bankBranchsService.createOrEdit(
				bankBranch
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBankBranchModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);