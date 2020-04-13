(function ($) {
    app.modals.CreateOrEditSimilarProductModal = function () {

        var _similarProductsService = abp.services.app.similarProducts;

        var _modalManager;
        var _$similarProductInformationForm = null;

		        var _SimilarProductproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/SimilarProducts/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/SimilarProducts/_SimilarProductProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$similarProductInformationForm = _modalManager.getModal().find('form[name=SimilarProductInformationsForm]');
            _$similarProductInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var similarProduct = _$similarProductInformationForm.serializeFormToObject();

            _SimilarProductproductLookupTableModal.open({ id: similarProduct.productId, displayName: similarProduct.productName }, function (data) {
                _$similarProductInformationForm.find('input[name=productName]').val(data.displayName); 
                _$similarProductInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$similarProductInformationForm.find('input[name=productName]').val(''); 
                _$similarProductInformationForm.find('input[name=productId]').val(''); 
        });
		
        $('#OpenProduct2LookupTableButton').click(function () {

            var similarProduct = _$similarProductInformationForm.serializeFormToObject();

            _SimilarProductproductLookupTableModal.open({ id: similarProduct.similarProductId, displayName: similarProduct.productName2 }, function (data) {
                _$similarProductInformationForm.find('input[name=productName2]').val(data.displayName); 
                _$similarProductInformationForm.find('input[name=similarProductId]').val(data.id); 
            });
        });
		
		$('#ClearProductName2Button').click(function () {
                _$similarProductInformationForm.find('input[name=productName2]').val(''); 
                _$similarProductInformationForm.find('input[name=similarProductId]').val(''); 
        });
		


        this.save = function () {
            if (!_$similarProductInformationForm.valid()) {
                return;
            }
            if ($('#SimilarProduct_ProductId').prop('required') && $('#SimilarProduct_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#SimilarProduct_SimilarProductId').prop('required') && $('#SimilarProduct_SimilarProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var similarProduct = _$similarProductInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _similarProductsService.createOrEdit(
				similarProduct
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditSimilarProductModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);