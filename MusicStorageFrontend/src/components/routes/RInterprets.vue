<template>
  <v-layout id="root">
    <!-- this component shows overall information about how the tool works and a small taxonomy of our method -->
    <v-flex>
      <v-card :dark="darkeningGeneral">
        <v-card-title>
          <h2>Interprets</h2>
          <v-spacer></v-spacer>
          <v-text-field
                  v-model="searchInterpret"
                  append-icon="search"
                  label="Search"
                  single-line
                  hide-details
                  style="margin-right: 15px"
          />
          <v-spacer></v-spacer>
          <div>
            <v-layout row wrap>
              <v-btn class="ma-2" tile large icon @click="openAddDialog()">
                <v-icon>add</v-icon>
              </v-btn>
            </v-layout>
          </div>
        </v-card-title>
        <v-data-table
                :headers="headers"
                :items="appStore.state.interpretsWithID"
                :loading="appStore.state.loadInterprets"
        >
          <v-progress-linear
                  color="blue"
                  indeterminate
                  slot="progress"
          />
          <template slot="items" slot-scope="props">
            <td class="text-xs-left tableData" @click="navigateToInterpretDetails(props.item)"><a>{{
              props.item.Name }}</a></td>
            <td class="text-xs-left tableData">
              <v-btn class="ma-2" tile large icon @click="editItem(props.item)">
                <v-icon>edit</v-icon>
              </v-btn>
              <v-btn
                      class="ma-2"
                      tile
                      large
                      icon
                      @click="deleteItem(props.item)"
              >
                <v-icon>delete</v-icon>
              </v-btn>
            </td>
          </template>
        </v-data-table>
      </v-card>
      <CAddInterpretDialog
              :dialog="openDialog"
              v-on:closed-Dialog="closeDialog"
              :interpretProp="interpret"
              :editMode="editDialog"
      />
      <CDeleteDialog
              :dialog="deleteDialog"
              v-on:closed-Dialog="closeDeleteDialog"
              :component="interpretDelete.Name"
              :id="interpretDelete.ID"
              :interpretMode="true"
      />
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
  import Vue from "vue";
  import Component from "vue-class-component";
  import {StateModule, AppStore} from "../../store/AppStore";
  import {VueStateField} from "../../store/State";
  import {Watch} from "vue-property-decorator";
  import CDeleteDialog from "../general/CDeleteDialog .vue";
  import IInterpret from "../../model/IInterpret";
  import CAddInterpretDialog from "../general/CAddInterpretDialog.vue";

  @Component({
    components: {CDeleteDialog, CAddInterpretDialog}
  })
  export default class RInterprets extends Vue {
    @VueStateField(StateModule.GENERAL)
    public darkeningGeneral: boolean;

    @VueStateField(StateModule.GENERAL)
    public searchInterpret: string;

    @VueStateField(StateModule.GENERAL)
    public sortByInterpret: string;

    appStore = AppStore;
    counterSort = 1;
    previousSort = "name";
    editDialog = false;
    deleteDialog = false;

    interpret = {
      Name: "",
      ID: null
    } as IInterpret;

    interpretDelete = {
      Name: "",
      ID: null
    } as IInterpret;

    headers = [
      {
        text: "Name",
        align: "left",
        value: "name",
        sortable: true
      },
      {text: "Actions", value: "actions", sortable: false}
    ];

    openDialog = false;

    public created() {
      AppStore.commit("loadInterprets");
    }

    mounted() {
      for (let i = 0; i < document.getElementsByTagName("th").length - 2; i++) {
        document.getElementsByTagName("th")[i].addEventListener(
          "click",
          event => {
            if (this.previousSort === this.headers[i].value) {
              switch (this.counterSort) {
                case 0:
                  this.sortByInterpret = this.headers[i].value;
                  this.counterSort++;
                  break;
                case 1:
                  this.sortByInterpret = "-" + this.headers[i].value;
                  this.counterSort++;
                  break;
                case 2:
                  this.sortByInterpret = this.headers[i].value;
                  this.counterSort = 0;
                  break;
              }
            } else {
              this.previousSort = this.headers[i].value;
              this.sortByInterpret = this.headers[i].value;
              this.counterSort = 1;
            }
          },
          true
        );
      }
    }

    public closeDialog(dialog) {
      this.openDialog = dialog;
    }

    public closeDeleteDialog(dialog) {
      this.deleteDialog = dialog;
    }

    openAddDialog() {
      this.interpret = {
        Name: "",
        ID: null
      } as IInterpret;
      this.openDialog = true;
      this.editDialog = false;
    }

    editItem(item: IInterpret) {
      this.editDialog = true;
      this.openDialog = true;
      this.interpret = Object.assign(item);
    }

    deleteItem(item: IInterpret) {
      this.deleteDialog = true;
      this.interpretDelete = item;
    }

    navigateToInterpretDetails(interpret: IInterpret) {
      this.$router.push("/interprets/" + interpret.ID);
    }

    @Watch("searchInterpret")
    public __searchMusic() {
      AppStore.commit("loadInterprets");
    }

    @Watch("sortByInterpret")
    public __sortMusic() {
      AppStore.commit("loadInterprets");
    }

    beforeDestroy() {
      this.searchInterpret = "";
      this.sortByInterpret = "name";
    }
  }
</script>

<style scoped>
  #root {
    height: 100%;
  }

  .tableData {
    -webkit-touch-callout: none;
    -webkit-user-select: none;
    -khtml-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
  }
</style>
