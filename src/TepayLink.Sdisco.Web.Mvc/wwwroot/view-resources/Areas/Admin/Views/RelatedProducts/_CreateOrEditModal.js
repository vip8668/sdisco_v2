(function ($) {
    app.modals.CreateOrEditRelatedProductModal = function () {

        var _relatedProductsService = abp.services.app.relatedProducts;

        var _modalManager;
        var _$relatedProductInformationForm = null;

		        var _RelatedProductproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/RelatedProducts/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/RelatedProducts/_RelatedProductProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$relatedProductInformationForm = _modalManager.getModal().find('form[name=RelatedProductInformationsForm]');
            _$relatedProductInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var relatedProduct = _$relatedProductInformationForm.serializeFormToObject();

            _RelatedProductproductLookupTableModal.open({ id: relatedProduct.productId, displayName: relatedProduct.productName }, function (data) {
                _$relatedProductInformationForm.find('input[name=productName]').val(data.displayName); 
                _$relatedProductInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$relatedProductInformationForm.find('input[name=productName]').val(''); 
                _$relatedProductInformationForm.find('input[name=productId]').val(''); 
        });
		
        $('#OpenProduct2LookupTableButton').click(function () {

            var relatedProduct = _$relatedProductInformationForm.serializeFormToObject();

            _RelatedProductproductLookupTableModal.open({ id: relatedProduct.relatedProductId, displayName: relatedProduct.productName2 }, function (data) {
                _$relatedProductInformationForm.find('input[name=productName2]').val(data.displayName); 
                _$relatedProductInformationForm.find('input[name=relatedProductId]').val(data.id); 
            });
        });
		
		$('#ClearProductName2Button').click(function () {
                _$relatedProductInformationForm.find('input[name=productName2]').val(''); 
                _$relatedProductInformationForm.find('input[name=relatedProductId]').val(''); 
        });
		


        this.save = function () {
            if (!_$relatedProductInformationForm.valid()) {
                return;
            }
            if ($('#RelatedProduct_ProductId').prop('required') && $('#RelatedProduct_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#RelatedProduct_RelatedProductId').prop('required') && $('#RelatedProduct_RelatedProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var relatedProduct = _$relatedProductInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _relatedProductsService.createOrEdit(
				relatedProduct
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRelatedProductModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);