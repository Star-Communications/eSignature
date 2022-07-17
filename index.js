const fs = require("fs");

fs.readFile("./normalJson.json", "utf8", (err, data) => {
    const canonical = Serialize(JSON.parse(data));

    fs.writeFile("./canonical.txt", canonical, (err) => {
        return;
    });
});

const Serialize = (documentStructure) => {
    if (typeof documentStructure !== "object") {
        return '"' + documentStructure + '"';
    }

    var serializedString = "";

    for (var element in documentStructure) {
        if (documentStructure[element].constructor !== Array) {
            serializedString += `"${element.toUpperCase()}"`;
            serializedString += Serialize(documentStructure[element]);
        } else {
            serializedString += `"${element.toUpperCase()}"`;
            documentStructure[element].forEach((arrayElement) => {
                serializedString += `"${element.toUpperCase()}"`;
                serializedString += Serialize(arrayElement);
            });
        }
    }

    return serializedString;
};
