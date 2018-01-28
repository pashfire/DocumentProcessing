"use strict";

var documentPageVueInstance;

document.addEventListener("DOMContentLoaded", function (event) {
    Vue.use(VuelidateErrorExtractor.default, {
        template: VuelidateErrorExtractor.templates.foundation6
    });

    documentPageVueInstance = new Vue({
        el: '#documentPage',
        data: () => ({
            form: {
                Name: null,
                CreatorId: null,
                Creator: null,
                Id: null,
                Path: null,
                CurrentResponsibleId: null,
                Responsible: null,
                Type: null,
                AllTypes: null,
                DocumentFile: null,
                PossibleUsersToAssign: null,
                Created: null,
            },
            
            editing: true,
            sending: false,
            lastUser: null
        }),
        computed: {
            orderedTypes: function () {
                return this.AllTypes
            }
        },

        mounted: function () {
            this.editing = !window.model.IsDocumentExists;
            this.form = window.model.DocumentModel;
            this.form.DocumentFile = null;
        },

        validations: {
            form: {
                Name: {
                    required: validators.required,
                    minLength: validators.minLength(5)
                },
                Type: {
                    required: validators.required,
                    between: validators.between(1, 99999999)
                },
                CurrentResponsibleId: {
                    required: validators.required,
                    between: validators.between(1, 99999999)
                },
                DocumentFile: {
                    required: validators.required
                }
            }
        },
        //computed: {
        //    orderedTypes: function () {
        //        return _.orderBy(this.AllTypes, 'Name')
        //    },

        methods: {
            getValidationClass(fieldName) {
                const field = this.$v.form[fieldName]

                if (field) {
                    return {
                        'md-invalid': field.$invalid && field.$dirty
                    }
                }
            },
            orderedTypes: function () {
                return _.orderBy(this.AllTypes, 'Name')
            },
            saveDocument() {
                this.sending = true;
                var _this = this;

                var formData = new FormData(document.getElementById('documentForm'));

                formData.append("Id", this.form.Id);
                formData.append("Name", this.form.Name);
                formData.append("CreatorId", this.form.CreatorId);
                formData.append("CurrentResponsibleId", this.form.CurrentResponsibleId);
                formData.append("Type", this.form.Type);
                

                Vue.http.post('/Document/Index', formData)
                    .then(response => {
                        //success callback
                        _this.documentCreated = true;
                        this.lastUser = `${this.form.firstName} ${this.form.lastName}`;
                        this.sending = false;
                        if (this.userSaved) {
                            document.location.href = "/";
                        }
                    }, response => {
                        // error callback
                        _this.documentCreated = false;
                        this.sending = false;
                    });
            },
            validateDocument() {
                this.$v.$touch();

                if (!this.$v.$invalid) {
                    this.saveDocument();
                }
            }
        }

    });
});