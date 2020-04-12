(function ($) {
    app.modals.CreateOrEditProductScheduleModal = function () {

        var _productSchedulesService = abp.services.app.productSchedules;

        var _modalManager;
        var _$productScheduleInformationForm = null;

		        var _ProductScheduleproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductSchedules/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductSchedules/_ProductScheduleProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productScheduleInformationForm = _modalManager.getModal().find('form[name=ProductScheduleInformationsForm]');
            _$productScheduleInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productSchedule = _$productScheduleInformationForm.serializeFormToObject();

            _ProductScheduleproductLookupTableModal.open({ id: productSchedule.productId, displayName: productSchedule.productName }, function (data) {
                _$productScheduleInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productScheduleInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productScheduleInformationForm.find('input[name=productName]').val(''); 
                _$productScheduleInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productScheduleInformationForm.valid()) {
                return;
            }
            if ($('#ProductSchedule_ProductId').prop('required') && $('#ProductSchedule_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var productSchedule = _$productScheduleInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productSchedulesService.createOrEdit(
				productSchedule
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductScheduleModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);