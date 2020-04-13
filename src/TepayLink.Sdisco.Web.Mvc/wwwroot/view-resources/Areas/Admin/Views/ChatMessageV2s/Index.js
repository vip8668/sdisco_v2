(function () {
    $(function () {

        var _$chatMessageV2sTable = $('#ChatMessageV2sTable');
        var _chatMessageV2sService = abp.services.app.chatMessageV2s;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ChatMessageV2s.Create'),
            edit: abp.auth.hasPermission('Pages.ChatMessageV2s.Edit'),
            'delete': abp.auth.hasPermission('Pages.ChatMessageV2s.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ChatMessageV2s/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ChatMessageV2s/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChatMessageV2Modal'
        });

		 var _viewChatMessageV2Modal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ChatMessageV2s/ViewchatMessageV2Modal',
            modalClass: 'ViewChatMessageV2Modal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$chatMessageV2sTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chatMessageV2sService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ChatMessageV2sTableFilter').val(),
					minChatConversationIdFilter: $('#MinChatConversationIdFilterId').val(),
					maxChatConversationIdFilter: $('#MaxChatConversationIdFilterId').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					messageFilter: $('#MessageFilterId').val(),
					minCreationTimeFilter:  getDateFilter($('#MinCreationTimeFilterId')),
					maxCreationTimeFilter:  getDateFilter($('#MaxCreationTimeFilterId')),
					sideFilter: $('#SideFilterId').val(),
					readStateFilter: $('#ReadStateFilterId').val(),
					receiverReadStateFilter: $('#ReceiverReadStateFilterId').val(),
					sharedMessageIdFilter: $('#SharedMessageIdFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewChatMessageV2Modal.open({ id: data.record.chatMessageV2.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.chatMessageV2.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteChatMessageV2(data.record.chatMessageV2);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "chatMessageV2.chatConversationId",
						 name: "chatConversationId"   
					},
					{
						targets: 2,
						 data: "chatMessageV2.userId",
						 name: "userId"   
					},
					{
						targets: 3,
						 data: "chatMessageV2.message",
						 name: "message"   
					},
					{
						targets: 4,
						 data: "chatMessageV2.creationTime",
						 name: "creationTime" ,
					render: function (creationTime) {
						if (creationTime) {
							return moment(creationTime).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "chatMessageV2.side",
						 name: "side"   ,
						render: function (side) {
							return app.localize('Enum_ChatSide_' + side);
						}
			
					},
					{
						targets: 6,
						 data: "chatMessageV2.readState",
						 name: "readState"   ,
						render: function (readState) {
							return app.localize('Enum_ChatMessageReadState_' + readState);
						}
			
					},
					{
						targets: 7,
						 data: "chatMessageV2.receiverReadState",
						 name: "receiverReadState"   ,
						render: function (receiverReadState) {
							return app.localize('Enum_ChatMessageReadState_' + receiverReadState);
						}
			
					},
					{
						targets: 8,
						 data: "chatMessageV2.sharedMessageId",
						 name: "sharedMessageId"   
					}
            ]
        });

        function getChatMessageV2s() {
            dataTable.ajax.reload();
        }

        function deleteChatMessageV2(chatMessageV2) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chatMessageV2sService.delete({
                            id: chatMessageV2.id
                        }).done(function () {
                            getChatMessageV2s(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewChatMessageV2Button').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _chatMessageV2sService
                .getChatMessageV2sToExcel({
				filter : $('#ChatMessageV2sTableFilter').val(),
					minChatConversationIdFilter: $('#MinChatConversationIdFilterId').val(),
					maxChatConversationIdFilter: $('#MaxChatConversationIdFilterId').val(),
					minUserIdFilter: $('#MinUserIdFilterId').val(),
					maxUserIdFilter: $('#MaxUserIdFilterId').val(),
					messageFilter: $('#MessageFilterId').val(),
					minCreationTimeFilter:  getDateFilter($('#MinCreationTimeFilterId')),
					maxCreationTimeFilter:  getDateFilter($('#MaxCreationTimeFilterId')),
					sideFilter: $('#SideFilterId').val(),
					readStateFilter: $('#ReadStateFilterId').val(),
					receiverReadStateFilter: $('#ReceiverReadStateFilterId').val(),
					sharedMessageIdFilter: $('#SharedMessageIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChatMessageV2ModalSaved', function () {
            getChatMessageV2s();
        });

		$('#GetChatMessageV2sButton').click(function (e) {
            e.preventDefault();
            getChatMessageV2s();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChatMessageV2s();
		  }
		});
    });
})();