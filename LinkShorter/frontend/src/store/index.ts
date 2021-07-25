import axios from "axios";
import Vue from "vue";
import Vuex from "vuex";
import UserShortlink from "@/interfaces/userShortlink";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    isLoggedIn: false,
    username: "",
    shortlinks: [] as UserShortlink[],
  },
  mutations: {
    setIsLoggedIn(state, value) {
      state.isLoggedIn = value;
    },
    setUsername(state, username) {
      state.username = username;
    },
    setShortlinks(state, shortlinks: UserShortlink[]) {
      state.shortlinks = shortlinks;
    },
  },
  actions: {
    async fetchShortlinks({ commit }) {
      const response = await axios.get("api/link/getuserspecificlinks", { withCredentials: true });

      if (response.status !== 200) {
        throw response;
      }

      const data = await response.data;
      commit("setShortlinks", data);
    },
  },
  modules: {},
  getters: {
    getIsLoggedIn: (state) => {
      return state.isLoggedIn;
    },
    getUsername: (state) => {
      return state.username;
    },
  },
});
