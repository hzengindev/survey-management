# Survey Management

This project created for boilerplate about survey or exam module. For now it's helping to survey management but you can change everything easily. 

The main project structure contains 4 projects. 
- asp.net core server
- dataverse customization
- react client
- pcf

First of all if you want to use this solution you should import dataverse customization solution into your organization. I gave an unmanaged ([solution](https://github.com/hzengindev/survey-management/blob/main/src/dataverse-customization/SurveyManagement_1_0_0_1.zip)). You can easily import this solution. This solution contains entities, forms, views and actions and your free to change everything.

If you want to run standalone you should use **asp.net core server** and **react client** project. asp.net core server app helps sed your survey data to the dataverse organization. react client app shows survey information to end users.

If you want to run in dataverse you should use **pcf control**. PCF Control works like react client and has a very user friendly UI.

## About Entities

### Survey
 Contains your main survey information.
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Description|MLT|Description |
|Pagination Type|OptionSet|You can define pagination type. (Order / Survey Pagination Group) |
|Record Per Page|Whole Number|You can define how many question show per page |
|Image|Image|You can define image for survey |

### Survey Question
 Contains your survey questions.
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Description|MLT|Description |
|Survey|Lookup|Survey Lookup |
|Type|OptionSet|Question types (Select / Multi Select / Text) |
|Required|Two Option|If you want to mark a question as mandatory, you can use this. |
|Survey Answer Option Template|Lookup|If you want to use the same answer options in more than one question, you can create answer option templates and bind it this lookup. |
|Order|Whole Number|Question order|
|Additional Answer|Two Option|If you want to more specific description you can use this. This option will show additional text area.|
|Image|Image|You can define image for survey question |

### Survey Question Group
 Contains your survey questions.
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Description|MLT|Description |
|Order|Whole Number|Order |
|Show Description|Two Option|Show Description |

### Survey Answer Option
 Contains your survey questions answer options.
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Order|Whole Number|Order |
|Image|Image|Answer image |
|Survey Question|Lookup|You should bind answer option to spesific question with lookup. But if you want to use Answer Option Template you should not fill this lookup. |
|Survey Answer Option Template|Lookup|If you create answer option for answer option template you should fill this lookup.  |

### Survey Answer Option Template
 Contains your answer options template.
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Survey|Lookup|Survey answer option template must be created for the survey |

### Survey Requests
 Survey request records created for survey session. You can create without any relationship or you can create to any entity relationship then you can create specific records. 
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Survey|Lookup|Survey|
|Responsible Firstname|SLT|Responsible Firstname|
|Responsible Lastname|SLT|Responsible Lastname|
|Responsible Email|SLT|Responsible Email|
|Completed|Two Option|Completed|
|Completion Date|Datetime|Completion Date|
|Code|SLT|Code field is a SLT. If you want to use the standalone version you must share survey request link with code. If you want to use in dataverse you don't have to fill.|

### Survey Responses
 Survey response records created for your survey answers.
|Field|Type|Description|
|--|--|--|
|Name|SLT|Name |
|Survey Request|Lookup|Survey Request|
|Survey Question|Lookup|Survey Question|
|Text Answer|SLT|Text Answer|
|Select Answer|Lookup|Select Answer (Lookup)|
|Multi Select Answer|Lookup|Multi Select Answer (Lookup)|
|Additional Answer|MLT|Additional Answer|

...
