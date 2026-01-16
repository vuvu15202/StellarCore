let selectedFunctionId = null;

export const SelectedFunctionStore = {
    set(value) {
        selectedFunctionId = value;
    },
    get() {
        return selectedFunctionId;
    }
};
