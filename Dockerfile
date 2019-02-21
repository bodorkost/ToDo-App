FROM node:10
WORKDIR /app/react
COPY react-front/package*.json ./
RUN npm install
COPY react-front/. .

EXPOSE 3000
CMD ["npm", "start"]