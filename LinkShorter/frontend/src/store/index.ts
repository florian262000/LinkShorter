import axios from "axios";
import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    isLoggedIn: false,
    username: "",
    shortlinks: [],
  },
  mutations: {
    setIsLoggedIn(state, value) {
      state.isLoggedIn = value;
    },
    setUsername(state, username) {
      state.username = username;
    },
    setShortlinks(state, shortlinks) {
      state.shortlinks = shortlinks;
    },
  },
  actions: {
    async fetchShortlinks({ commit }) {
      const response = await axios.post(
        "api/link/getuserspecificlinks",
        {},
        {
          withCredentials: true,
        }
      );

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
