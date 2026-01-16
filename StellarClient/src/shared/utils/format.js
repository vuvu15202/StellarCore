import dayjs from 'dayjs';
import * as numeral from 'numeral';

if (numeral.locales['vi'] === undefined) {
    numeral.register('locale', 'vi', {
        delimiters: {
            thousands: '.',
            decimal: ','
        },
        abbreviations: {
            thousand: 'k',
            million: 'm',
            billion: 'b',
            trillion: 't'
        },
        ordinal: function (number) {
            return number === 1 ? 'er' : 'ème';
        },
        currency: {
            symbol: 'VNĐ'
        }
    });
}


// switch between locales
numeral.locale('vi');

// Date
function formatDate(value, type = 'DD/MM/YYYY') {
    if (value) {
        let result = '';
        try {
            result = dayjs(value).format(type);
            // eslint-disable-next-line no-empty
        } catch (e) {
        }
        return result;
    } else {
        return '';
    }
}

// Time
function formatTime(value, type = 'HH:mm') {
    if (value) {
        let result = '';
        try {
            result = dayjs(value).format(type);
            // eslint-disable-next-line no-empty
        } catch (e) {
        }
        return result;
    } else {
        return '';
    }
}

function formatDecimal(v) {
    if (v) {
        return numeral(v).format('0,0[.]0000');
    }
    return v;
}

function formatInt(v) {
    if (v) {
        return numeral(v).format('0,0');
    }
    return v;
}

function parserInt(v) {
    if (v) {
        return numeral(v).value();
    }
    return v;
}

function formatNumber(v, f) {
    const formatStr = f || '0,0[.]0000';
    if (v) {
        return numeral(v).format(formatStr);
    }
    return v;
}

const integerToRoman = (num) => {
    const romanValues = {
        M: 1000,
        CM: 900,
        D: 500,
        CD: 400,
        C: 100,
        XC: 90,
        L: 50,
        XL: 40,
        X: 10,
        IX: 9,
        V: 5,
        IV: 4,
        I: 1
    };
    let roman = '';
    for (let key in romanValues) {
        while (num >= romanValues[key]) {
            roman += key;
            num -= romanValues[key];
        }
    }
    return roman;
};


function deepTrim(object) {
    if (typeof object === 'string') {
        return object.trim();
    } else if (typeof object === 'object') {
        for (var key in object) {
            try {
                object[key] = deepTrim(object[key]);
            } catch {
                // empty
            }

        }
    }

    return object;
}

const removeTrimData = (data) => {
    return deepTrim(data);
};


export {
    formatDate,
    formatDecimal,
    formatInt,
    numeral,
    formatTime,
    integerToRoman,
    parserInt,
    removeTrimData,
    formatNumber
};