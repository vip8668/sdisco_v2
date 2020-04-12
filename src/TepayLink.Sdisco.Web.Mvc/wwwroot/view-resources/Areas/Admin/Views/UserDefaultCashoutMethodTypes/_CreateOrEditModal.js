(function ($) {
    app.modals.CreateOrEditUserDefaultCashoutMethodTypeModal = function () {

        var _userDefaultCashoutMethodTypesService = abp.services.app.userDefaultCashoutMethodTypes;

        var _modalManager;
        var _$userDefaultCashoutMethodTypeInformationForm = null;

		        var _UserDefaultCashoutMethodTypecashoutMethodTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserDefaultCashoutMethodTypes/CashoutMethodTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/UserDefaultCashoutMethodTypes/_UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableModal.js',
            modalClass: 'CashoutMethodTypeLookupTableModal'
        });        var _UserDefaultCashoutMethodTypeuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/UserDefaultCashoutMethodTypes/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/UserDefaultCashoutMethodTypes/_UserDefaultCashoutMethodTypeUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$userDefaultCashoutMethodTypeInformationForm = _modalManager.getModal().find('form[name=UserDefaultCashoutMethodTypeInformationsForm]');
            _$userDefaultCashoutMethodTypeInformationForm.validate();
        };

		          $('#OpenCashoutMethodTypeLookupTableButton').click(function () {

            var userDefaultCashoutMethodType = _$userDefaultCashoutMethodTypeInformationForm.serializeFormToObject();

            _UserDefaultCashoutMethodTypecashoutMethodTypeLookupTableModal.open({ id: userDefaultCashoutMethodType.cashoutMethodTypeId, displayName: userDefaultCashoutMethodType.cashoutMethodTypeTitle }, function (data) {
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=cashoutMethodTypeTitle]').val(data.displayName); 
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=cashoutMethodTypeId]').val(data.id); 
            });
        });
		
		$('#ClearCashoutMethodTypeTitleButton').click(function () {
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=cashoutMethodTypeTitle]').val(''); 
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=cashoutMethodTypeId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var userDefaultCashoutMethodType = _$userDefaultCashoutMethodTypeInformationForm.serializeFormToObject();

            _UserDefaultCashoutMethodTypeuserLookupTableModal.open({ id: userDefaultCashoutMethodType.userId, displayName: userDefaultCashoutMethodType.userName }, function (data) {
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=userName]').val(data.displayName); 
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=userName]').val(''); 
                _$userDefaultCashoutMethodTypeInformationForm.find('input[name=userId]').val(''); 
        });
		


        this.save = function () {
            if (!_$userDefaultCashoutMethodTypeInformationForm.valid()) {
                return;
            }
            if ($('#UserDefaultCashoutMethodType_CashoutMethodTypeId').prop('required') && $('#UserDefaultCashoutMethodType_CashoutMethodTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('CashoutMethodType')));
                return;
            }
            if ($('#UserDefaultCashoutMethodType_UserId').prop('required') && $('#UserDefaultCashoutMethodType_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var userDefaultCashoutMethodType = _$userDefaultCashoutMethodTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _userDefaultCashoutMethodTypesService.createOrEdit(
				userDefaultCashoutMethodType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditUserDefaultCashoutMethodTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);