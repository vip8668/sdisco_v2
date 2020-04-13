(function ($) {
    app.modals.CreateOrEditSuggestedProductModal = function () {

        var _suggestedProductsService = abp.services.app.suggestedProducts;

        var _modalManager;
        var _$suggestedProductInformationForm = null;

		        var _SuggestedProductproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SuggestedProducts/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/SuggestedProducts/_SuggestedProductProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$suggestedProductInformationForm = _modalManager.getModal().find('form[name=SuggestedProductInformationsForm]');
            _$suggestedProductInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var suggestedProduct = _$suggestedProductInformationForm.serializeFormToObject();

            _SuggestedProductproductLookupTableModal.open({ id: suggestedProduct.productId, displayName: suggestedProduct.productName }, function (data) {
                _$suggestedProductInformationForm.find('input[name=productName]').val(data.displayName); 
                _$suggestedProductInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$suggestedProductInformationForm.find('input[name=productName]').val(''); 
                _$suggestedProductInformationForm.find('input[name=productId]').val(''); 
        });
		
        $('#OpenProduct2LookupTableButton').click(function () {

            var suggestedProduct = _$suggestedProductInformationForm.serializeFormToObject();

            _SuggestedProductproductLookupTableModal.open({ id: suggestedProduct.suggestedProductId, displayName: suggestedProduct.productName2 }, function (data) {
                _$suggestedProductInformationForm.find('input[name=productName2]').val(data.displayName); 
                _$suggestedProductInformationForm.find('input[name=suggestedProductId]').val(data.id); 
            });
        });
		
		$('#ClearProductName2Button').click(function () {
                _$suggestedProductInformationForm.find('input[name=productName2]').val(''); 
                _$suggestedProductInformationForm.find('input[name=suggestedProductId]').val(''); 
        });
		


        this.save = function () {
            if (!_$suggestedProductInformationForm.valid()) {
                return;
            }
            if ($('#SuggestedProduct_ProductId').prop('required') && $('#SuggestedProduct_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#SuggestedProduct_SuggestedProductId').prop('required') && $('#SuggestedProduct_SuggestedProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var suggestedProduct = _$suggestedProductInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _suggestedProductsService.createOrEdit(
				suggestedProduct
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditSuggestedProductModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);