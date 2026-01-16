import {SelectedFunctionStore} from 'shared/services/SelectedFunctionStore';
import {createContext, useEffect, useState} from "react";

export const SelectedFunctionContext = createContext(null);
export const SelectedFunctionProvider = ({children}) => {
    const [selectedFunctionId, setSelectedFunctionId] = useState(null);

    useEffect(() => {
        SelectedFunctionStore.set(selectedFunctionId);
    }, [selectedFunctionId]);

    return (
        <SelectedFunctionContext.Provider value={{selectedFunctionId, setSelectedFunctionId}}>
            {children}
        </SelectedFunctionContext.Provider>
    );
};