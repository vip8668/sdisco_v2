(function ($) {
    app.modals.CreateOrEditBankAccountInfoModal = function () {

        var _bankAccountInfosService = abp.services.app.bankAccountInfos;

        var _modalManager;
        var _$bankAccountInfoInformationForm = null;

		        var _BankAccountInfobankLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankAccountInfos/BankLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BankAccountInfos/_BankAccountInfoBankLookupTableModal.js',
            modalClass: 'BankLookupTableModal'
        });        var _BankAccountInfobankBranchLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankAccountInfos/BankBranchLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BankAccountInfos/_BankAccountInfoBankBranchLookupTableModal.js',
            modalClass: 'BankBranchLookupTableModal'
        });        var _BankAccountInfouserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BankAccountInfos/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BankAccountInfos/_BankAccountInfoUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bankAccountInfoInformationForm = _modalManager.getModal().find('form[name=BankAccountInfoInformationsForm]');
            _$bankAccountInfoInformationForm.validate();
        };

		          $('#OpenBankLookupTableButton').click(function () {

            var bankAccountInfo = _$bankAccountInfoInformationForm.serializeFormToObject();

            _BankAccountInfobankLookupTableModal.open({ id: bankAccountInfo.bankId, displayName: bankAccountInfo.bankBankName }, function (data) {
                _$bankAccountInfoInformationForm.find('input[name=bankBankName]').val(data.displayName); 
                _$bankAccountInfoInformationForm.find('input[name=bankId]').val(data.id); 
            });
        });
		
		$('#ClearBankBankNameButton').click(function () {
                _$bankAccountInfoInformationForm.find('input[name=bankBankName]').val(''); 
                _$bankAccountInfoInformationForm.find('input[name=bankId]').val(''); 
        });
		
        $('#OpenBankBranchLookupTableButton').click(function () {

            var bankAccountInfo = _$bankAccountInfoInformationForm.serializeFormToObject();

            _BankAccountInfobankBranchLookupTableModal.open({ id: bankAccountInfo.bankBranchId, displayName: bankAccountInfo.bankBranchBranchName }, function (data) {
                _$bankAccountInfoInformationForm.find('input[name=bankBranchBranchName]').val(data.displayName); 
                _$bankAccountInfoInformationForm.find('input[name=bankBranchId]').val(data.id); 
            });
        });
		
		$('#ClearBankBranchBranchNameButton').click(function () {
                _$bankAccountInfoInformationForm.find('input[name=bankBranchBranchName]').val(''); 
                _$bankAccountInfoInformationForm.find('input[name=bankBranchId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var bankAccountInfo = _$bankAccountInfoInformationForm.serializeFormToObject();

            _BankAccountInfouserLookupTableModal.open({ id: bankAccountInfo.userId, displayName: bankAccountInfo.userName }, function (data) {
                _$bankAccountInfoInformationForm.find('input[name=userName]').val(data.displayName); 
                _$bankAccountInfoInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$bankAccountInfoInformationForm.find('input[name=userName]').val(''); 
                _$bankAccountInfoInformationForm.find('input[name=userId]').val(''); 
        });
		


        this.save = function () {
            if (!_$bankAccountInfoInformationForm.valid()) {
                return;
            }
            if ($('#BankAccountInfo_BankId').prop('required') && $('#BankAccountInfo_BankId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Bank')));
                return;
            }
            if ($('#BankAccountInfo_BankBranchId').prop('required') && $('#BankAccountInfo_BankBranchId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('BankBranch')));
                return;
            }
            if ($('#BankAccountInfo_UserId').prop('required') && $('#BankAccountInfo_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var bankAccountInfo = _$bankAccountInfoInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bankAccountInfosService.createOrEdit(
				bankAccountInfo
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBankAccountInfoModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);