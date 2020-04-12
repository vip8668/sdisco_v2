(function ($) {
    app.modals.CreateOrEditSaveItemModal = function () {

        var _saveItemsService = abp.services.app.saveItems;

        var _modalManager;
        var _$saveItemInformationForm = null;

		        var _SaveItemproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SaveItems/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/SaveItems/_SaveItemProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$saveItemInformationForm = _modalManager.getModal().find('form[name=SaveItemInformationsForm]');
            _$saveItemInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var saveItem = _$saveItemInformationForm.serializeFormToObject();

            _SaveItemproductLookupTableModal.open({ id: saveItem.productId, displayName: saveItem.productName }, function (data) {
                _$saveItemInformationForm.find('input[name=productName]').val(data.displayName); 
                _$saveItemInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$saveItemInformationForm.find('input[name=productName]').val(''); 
                _$saveItemInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$saveItemInformationForm.valid()) {
                return;
            }
            if ($('#SaveItem_ProductId').prop('required') && $('#SaveItem_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var saveItem = _$saveItemInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _saveItemsService.createOrEdit(
				saveItem
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditSaveItemModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);