"use strict";
var documentPageVueInstance;


document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    documentPageVueInstance = new Vue({
        el: '#documentsPage',
        data: () => ({
            docs: {
                AllDocuments: null,
                Id: null,
                Document: null,
            },
            editing: true,
            sending: false,
            lastUser: null
        }),

        mounted: function () {
            this.editing = !window.model.IsDocumentExists;
            this.docs = window.model.AllDocumentsModel;
        },

        methods: {
            
            orderedTypes: function () {
                return _.orderBy(this.AllTypes, 'Name')
            },
            toLower(text) {
                return text.toString().toLowerCase()
            },

            searchByName(items, term) {
                if (term) {
                    return items.filter(function (item) {
                        return toLower(item.name).includes(toLower(term))
                    })
                }

                return items
            },

            searchOnTable: function searchOnTable() {
                this.searched = searchByName(this.users, this.search)
            },
        }

    });
});