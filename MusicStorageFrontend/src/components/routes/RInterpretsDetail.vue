<template>
  <v-layout id="root">
    <!-- this component shows overall information about how the tool works and a small taxonomy of our method -->
    <v-flex>
      <v-card :dark="darkeningGeneral">
        <v-card-title>
          <v-btn
                  class="ma-2"
                  tile
                  large
                  icon
                  @click="backToPreviousRoute()"
          >
            <v-icon>keyboard_backspace</v-icon>
          </v-btn>
          <h2>{{appStore.state.interpret.Name}}</h2>
          <v-spacer/>
          <v-text-field
                  v-model="searchMusicFile"
                  append-icon="search"
                  label="Search"
                  single-line
                  hide-details
                  style="margin-right: 15px"
          />
          <v-spacer/>
          <div>
            <v-layout row wrap>
              <v-btn class="ma-2" tile large icon @click="openAddDialog()">
                <v-icon>add</v-icon>
              </v-btn>
              <v-btn
                      class="ma-2"
                      tile
                      large
                      icon
                      @click="deleteInterpret()"
              >
                <v-icon>delete</v-icon>
              </v-btn>
            </v-layout>
          </div>
        </v-card-title>
        <v-data-table
                :headers="headers"
                :items="appStore.state.musicFilesWithoutIDs"
                :loading="appStore.state.loadingMusicFiles"
        >
          <v-progress-linear
                  color="blue"
                  indeterminate
                  slot="progress"
          />
          <template slot="items" slot-scope="props">
            <td class="text-xs-left tableData">{{ props.item.Title }}</td>
            <td class="text-xs-left tableData">
              {{ props.item.Interpret }}
            </td>
            <td class="text-xs-left tableData">
              {{ props.item.Genre }}
            </td>
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
      <CAddMusicFileDialog
              :dialog="openDialog"
              v-on:closed-Dialog="closeDialog"
              :musicFileProp="musicFile"
              :editMode="editDialog"
              :interpretMode="true"
      />
      <CDeleteDialog
              :dialog="deleteDialog"
              v-on:closed-Dialog="closeDeleteDialog"
              :component="musicFileDelete.Title"
              :id="musicFileDelete.ID"
              :editMode="editDialog"
              :interpretMode="false"
      />
      <CDeleteDialog
              :dialog="deleteDialogInterpret"
              v-on:closed-Dialog="closeDeleteDialogInterpret"
              :component="appStore.state.interpret.Name"
              :id="appStore.state.interpret.ID"
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
  import CFilterSidebar from "../general/CFilterSidebar.vue";
  import CAddMusicFileDialog from "../general/CAddMusicFileDialog.vue";
  import {IMusicFileWithoutIDs} from "../../model/IMusicFileWithoutIDs";
  import CDeleteDialog from "../general/CDeleteDialog .vue";

  @Component({
    components: {CFilterSidebar, CAddMusicFileDialog, CDeleteDialog}
  })
  export default class RInterpretsDetail extends Vue {
    @VueStateField(StateModule.GENERAL)
    public darkeningGeneral: boolean;

    @VueStateField(StateModule.GENERAL)
    public searchMusicFile: string;

    @VueStateField(StateModule.GENERAL)
    public sortByMusicFile: string;

    appStore = AppStore;
    counterSort = 1;
    previousSort = "title";
    editDialog = false;
    deleteDialog = false;
    deleteDialogInterpret = false;
    prevRoute = null;

    musicFile = {
      Title: "",
      Interpret: "",
      Genre: "",
      ID: null
    } as IMusicFileWithoutIDs;

    musicFileDelete = {
      Title: "",
      Interpret: "",
      Genre: "",
      ID: null
    } as IMusicFileWithoutIDs;

    headers = [
      {
        text: "Title",
        align: "left",
        value: "title",
        sortable: true
      },
      {text: "Interpret", value: "interpret", sortable: true},
      {text: "Genre", value: "genre", sortable: true},
      {text: "Actions", value: "actions", sortable: false}
    ];

    openDialog = false;

    public created() {
      AppStore.commit("loadInterpret", this.$route.params.id);
    }

    mounted() {
      for (let i = 0; i < document.getElementsByTagName("th").length - 2; i++) {
        document.getElementsByTagName("th")[i].addEventListener(
          "click",
          event => {
            if (this.previousSort === this.headers[i].value) {
              switch (this.counterSort) {
                case 0:
                  this.sortByMusicFile = this.headers[i].value;
                  this.counterSort++;
                  break;
                case 1:
                  this.sortByMusicFile = "-" + this.headers[i].value;
                  this.counterSort++;
                  break;
                case 2:
                  this.sortByMusicFile = this.headers[i].value;
                  this.counterSort = 0;
                  break;
              }
            } else {
              this.previousSort = this.headers[i].value;
              this.sortByMusicFile = this.headers[i].value;
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

    public closeDeleteDialogInterpret(dialog) {
      this.deleteDialogInterpret = dialog;
    }

    openAddDialog() {
      this.musicFile = {
        Title: "",
        Interpret: AppStore.state.interpret.Name.toString(),
        Genre: "",
        ID: null
      } as IMusicFileWithoutIDs;
      this.openDialog = true;
      this.editDialog = false;
    }

    editItem(item: IMusicFileWithoutIDs) {
      this.editDialog = true;
      this.openDialog = true;
      this.musicFile = Object.assign(item);
    }

    deleteItem(item: IMusicFileWithoutIDs) {
      this.deleteDialog = true;
      this.musicFileDelete = item;
    }

    deleteInterpret() {
      this.deleteDialogInterpret = true;
    }

    backToPreviousRoute() {
      this.$router.go(-1);
    }

    @Watch("searchMusicFile")
    public __searchMusic() {
      AppStore.commit("loadMusicFiles");
    }

    @Watch("sortByMusicFile")
    public __sortMusic() {
      AppStore.commit("loadMusicFiles");
    }

    @Watch("appStore.state.filterMusicFile", { immediate: true, deep: true })
    public __loadMusicFiles() {
      AppStore.commit("loadMusicFiles");
    }

    beforeDestroy() {
      AppStore.commit("resetFilterMusicFile");
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
