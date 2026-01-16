export const ZustandPageStore = (set) => {

    return {
        data: [],
        metadata: {
            page: 1,
            page_size: 20,
            sort: undefined,
            search: undefined,
            filter: undefined,
            total: 0,
        },
        actions: {
            setData: (data) => {
                set(() => {
                    return {data};
                });
            },
            setMetadata: (value) => {
                set((state) => {

                    return {metadata: {...state.metadata, ...value}};
                });
            },
            resetMetadata: () => {
                set(() => {
                    return {
                        metadata: {
                            page: 1,
                            page_size: 20,
                            sort: undefined,
                            filter: undefined,
                            search: undefined
                        }
                    };
                });
            }
        }
    };
};